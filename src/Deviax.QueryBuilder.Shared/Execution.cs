using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder
{
    public interface INameResolver
    {
        string DbToCSharp(string dbName);
        string CSharpToDb(string csharpName);
    }

    public static class AssignmentCache<T> where T : new()
    {
        public static Action<DbDataReader, T>? Action;
        public static Func<T>? Constructor;
    }

    public abstract class QueryExecutor
    {
        public static QueryExecutor DefaultExecutor = null!;
        public static INameResolver NameResolver => DefaultExecutor.DefaultNameResolver;

        protected abstract INameResolver DefaultNameResolver { get; }

        private static readonly TypeConversionSpecialization DefaultTypeConversionSpeciliation = new DefaultTypeConversionSpecialization();
        private static IEnumerable<TypeConversionSpecialization> TypeConversionSpecializationsWithDefault
        {
            get
            {
                foreach (var tcs in TypeConversionSpecializations)
                    yield return tcs;

                yield return DefaultTypeConversionSpeciliation;
            }
        }

        private static readonly List<TypeConversionSpecialization> TypeConversionSpecializations = new List<TypeConversionSpecialization> {
            new ListTypeConversionSpecialization()
        };
        
        internal static void RegisterTypeConversionSpecialization(TypeConversionSpecialization tcs)
        {
            TypeConversionSpecializations.Add(tcs);
        }

        private void Process<TVisitor, TQuery>(TVisitor v, TQuery q, IVisitorResult r) where TVisitor : BaseVisitor, IQueryingVisitor<TQuery>
        {
            r.Start();
            v.Result = r;
            v.Process(q);
            r.Finished();
        }
        
        private DbCommand ToCommand<TVisitor, TQuery>(TQuery q, DbConnection con, DbTransaction? tx = null) where TVisitor : BaseVisitor, IQueryingVisitor<TQuery>, new()
        {
            var r = new ActualCommandResult(con);
            Process(new TVisitor(), q, r);
            r.Command.Transaction = tx;
            return r.Command;
        }
        
        private string ToQueryText<TVisitor, TQuery>(TQuery q) where TVisitor : BaseVisitor, IQueryingVisitor<TQuery>, new()
        {
            var r = new ToSqlResult();
            Process(new TVisitor(), q, r);
            return r.StringBuilder + r.ParameterDescription;
        }
        
        public DbCommand ToCommand(BaseSelectQuery query, DbConnection con, DbTransaction? tx = null) 
            => ToCommand<SelectVisitor, BaseSelectQuery>(query, con, tx);

        public PreparedCommand ToPreparedCommand(BaseSelectQuery query)
        {
            var r = new PreparingCommandResult();
            Process(new SelectVisitor(), query, r);
            return r.Result;
        }

        public DbCommand ToCommand(BaseUpdateQuery query, DbConnection con, DbTransaction? tx = null)
            => ToCommand<UpdateVisitor, BaseUpdateQuery>(query, con, tx);
        
        public DbCommand ToCommand(BaseInsertQuery query, DbConnection con, DbTransaction? tx = null)
            => ToCommand<InsertVisitor, BaseInsertQuery>(query, con, tx);

        public DbCommand ToCommand(BaseDeleteQuery query, DbConnection con, DbTransaction? tx = null)
            => ToCommand<DeleteVisitor, BaseDeleteQuery>(query, con, tx);

        public string ToQueryText(BaseSelectQuery query)
            => ToQueryText<SelectVisitor, BaseSelectQuery>(query);

        public string ToQueryText(BaseDeleteQuery query)
            => ToQueryText<DeleteVisitor, BaseDeleteQuery>(query);

        public string ToQueryText(BaseInsertQuery query)
            => ToQueryText<InsertVisitor, BaseInsertQuery>(query);
        
        public string ToQueryText(BaseUpdateQuery query)
            => ToQueryText<UpdateVisitor, BaseUpdateQuery>(query);
        
        private static Action<DbDataReader, T> GenerateAssignment<T>(INameResolver nameResolver, DbDataReader reader) where T : new()
        {
            var expressions = new List<Expression>();

            var t = typeof(T);
            var targetParam = Expression.Parameter(t);
            var valueVariable = Expression.Variable(typeof(object));
            var readerParam = Expression.Parameter(typeof(DbDataReader));

            var getValueMethod = typeof(DbDataReader).GetTypeInfo().GetMethod("GetValue");

            #if DEBUG
            var exceptionConstructor = typeof(InvalidCastException).GetConstructor(new [] {typeof(string), typeof(Exception)})!;
            var exceptParam = Expression.Parameter(typeof(Exception));
            #endif
            
            for (var i = 0; i < reader.VisibleFieldCount; i++)
            {
                var dbName = reader.GetName(i);
                var csharpName = nameResolver.DbToCSharp(dbName);

                expressions.Add(Expression.Assign(valueVariable, Expression.Call(readerParam, getValueMethod!, Expression.Constant(i))));

                Expression notNullBranch = null!;

                if (csharpName == t.Name)
                {
                    csharpName += "F";
                }

                var field = t.GetTypeInfo().GetField(csharpName);

                if (field != null)
                {
                    var fi = field.FieldType.GetTypeInfo();
                    
                    foreach (var tcs in TypeConversionSpecializationsWithDefault)
                    {
                        if (tcs.Matches(fi, field.FieldType))
                        {
#if DEBUG
                            notNullBranch = Expression.TryCatch(
                                Expression.Assign(
                                    Expression.Field(targetParam, field),
                                    tcs.Convert(fi, field.FieldType, valueVariable)
                                    ),
                                Expression.Catch(exceptParam, Expression.Throw(Expression.New(exceptionConstructor,  Expression.Constant(field.Name), exceptParam ), field.FieldType))
                            );
#else
                            notNullBranch = Expression.Assign(
                                Expression.Field(targetParam, field),
                                tcs.Convert(fi, field.FieldType, valueVariable)
                            );
#endif
                            break;
                        }
                    }
                }
                else
                {

                    var property = t.GetTypeInfo().GetProperty(csharpName);

                    if (property == null)
                    {
                        throw new ArgumentException($"field or property {csharpName} on type {t} not found for database column {dbName}");
                    }

                    var ti = property.PropertyType.GetTypeInfo();
                    
                    foreach (var tcs in TypeConversionSpecializationsWithDefault)
                    {
                        if (tcs.Matches(ti, property.PropertyType))
                        {
#if DEBUG
                            notNullBranch = Expression.TryCatch( 
                                Expression.Assign(
                                    Expression.Property(targetParam, property),
                                    tcs.Convert(ti, property.PropertyType, valueVariable)
                                ),
                                Expression.Catch(exceptParam, Expression.Throw(Expression.New(exceptionConstructor,  Expression.Constant(property.Name), exceptParam ), property.PropertyType))
                            );
#else
                            notNullBranch = Expression.Assign(
                                Expression.Property(targetParam, property),
                                tcs.Convert(ti, property.PropertyType, valueVariable)
                            );
#endif
                            break;
                        }
                    }
                }

                var dbNullTest = Expression.Equal(valueVariable, Expression.Constant(DBNull.Value));

                expressions.Add(Expression.IfThen(Expression.IsFalse(dbNullTest), notNullBranch));
            }

            var block = Expression.Block(
                new[] {
                    valueVariable
                },
                expressions.ToArray()
            );

            var action = Expression.Lambda<Action<DbDataReader, T>>(block, readerParam, targetParam).Compile();
            return action;
        }

        public async Task<List<T>> ToList<T>(DbCommand cmd, bool prepare = true) where T : new()
        {
            var result = new List<T>();

            if (AssignmentCache<T>.Constructor == null)
            {
                AssignmentCache<T>.Constructor = GenerateConstructor<T>();
            }

            if (prepare)
            {
                await cmd.PrepareAsync();
            }

            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                if (AssignmentCache<T>.Action == null)
                {
                    AssignmentCache<T>.Action = GenerateAssignment<T>(DefaultNameResolver, reader);
                }

                while (reader.Read())
                {
                    var item = AssignmentCache<T>.Constructor();
                    AssignmentCache<T>.Action(reader, item);
                    result.Add(item);
                }
            }

            return result;
        }

        private Func<T> GenerateConstructor<T>() where T : new()
        {
            var constructor = typeof(T).GetConstructor(Type.EmptyTypes)!;

            return Expression.Lambda<Func<T>>(Expression.New(constructor)).Compile();
        }

        public List<T> ToListSync<T>(DbCommand cmd, bool prepare = true) where T : new()
        {
            var result = new List<T>();

            if (AssignmentCache<T>.Constructor == null)
            {
                AssignmentCache<T>.Constructor = GenerateConstructor<T>();
            }

            if (prepare)
            {
                cmd.Prepare();
            }

            using (var reader = cmd.ExecuteReader())
            {
                if (AssignmentCache<T>.Action == null)
                {
                    AssignmentCache<T>.Action = GenerateAssignment<T>(DefaultNameResolver, reader);
                }

                while (reader.Read())
                {
                    var item = AssignmentCache<T>.Constructor();
                    AssignmentCache<T>.Action(reader, item);
                    result.Add(item);
                }
            }

            return result;
        }

        public async Task ForEach<T>(DbCommand cmd, Action<T> action, bool prepare = true) where T : new()
        {
            if (AssignmentCache<T>.Constructor == null)
            {
                AssignmentCache<T>.Constructor = GenerateConstructor<T>();
            }

            if (prepare)
            {
                await cmd.PrepareAsync();
            }

            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                if (AssignmentCache<T>.Action == null)
                {
                    AssignmentCache<T>.Action = GenerateAssignment<T>(DefaultNameResolver, reader);
                }

                while (reader.Read())
                {
                    var item = AssignmentCache<T>.Constructor();
                    AssignmentCache<T>.Action(reader, item);
                    action(item);
                }
            }
        }

        public async Task ForEach<T>(DbCommand cmd, Func<T, Task> action, bool prepare = true) where T : new()
        {
            if (AssignmentCache<T>.Constructor == null)
            {
                AssignmentCache<T>.Constructor = GenerateConstructor<T>();
            }

            if (prepare)
            {
                await cmd.PrepareAsync();
            }

            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                if (AssignmentCache<T>.Action == null)
                {
                    AssignmentCache<T>.Action = GenerateAssignment<T>(DefaultNameResolver, reader);
                }

                while (reader.Read())
                {
                    var item = AssignmentCache<T>.Constructor();
                    AssignmentCache<T>.Action(reader, item);
                    await action(item);
                }
            }
        }

        public async Task InsertBatched<T>(T[] items, int batchSize, DbConnection con, DbTransaction? tx = null)
        {
            if (items.Length <= batchSize)
            {
                await Insert(items, con, tx);
            }
            else
            {
                foreach (var batch in items.Select((item, inx) => new {item, inx})
                    .GroupBy(x => x.inx / batchSize)
                    .Select(g => g.Select(x => x.item)))
                {
                    await Insert(batch.ToArray(), con, tx);
                }
            }
        }
        
        public void InsertBatchedSync<T>(T[] items, int batchSize, DbConnection con, DbTransaction? tx = null)
        {
            if (items.Length <= batchSize)
            {
                InsertSync(items, con, tx);
            }
            else
            {
                foreach (var batch in items.Select((item, inx) => new {item, inx})
                    .GroupBy(x => x.inx / batchSize)
                    .Select(g => g.Select(x => x.item)))
                {
                    InsertSync(batch.ToArray(), con, tx);
                }
            }
        }

        public async Task Insert<T>(T[] items, DbConnection con, DbTransaction? tx = null)
        {
            var table = TypeToTableEntry<T>.DefaultTable;
            var q = new BaseInsertQuery(table).WithValues(items.Select((item, i) => TypeToTableEntry<T>.ToValues(item!, table, i)).ToArray());
            var returningParts = TypeToTableEntry<T>.Returning(items[0]!, table);
            if (returningParts != null)
            {
                q = q.Returning(returningParts);
                var ids = await q.ScalarList<int>(con, tx, items.Length == 1);

                if (ids.Count != items.Length)
                    throw new InvalidOperationException();

                for (int i = 0; i < items.Length; i++)
                {
                    TypeToTableEntry<T>.ApplyReturning(items[i]!, ids[i]);
                }
            }
            else
            {
                await q.Execute(con, tx, items.Length == 1);
            }
        }
        
        public void InsertSync<T>(T[] items, DbConnection con, DbTransaction? tx = null)
        {
            var table = TypeToTableEntry<T>.DefaultTable;
            var q = new BaseInsertQuery(table).WithValues(items.Select((item, i) => TypeToTableEntry<T>.ToValues(item!, table, i)).ToArray());
            var returningParts = TypeToTableEntry<T>.Returning(items[0]!, table);
            if (returningParts != null)
            {
                q = q.Returning(returningParts);
                var ids = q.ScalarListSync<int>(con, tx, items.Length == 1);

                if (ids.Count != items.Length)
                    throw new InvalidOperationException();

                for (int i = 0; i < items.Length; i++)
                {
                    TypeToTableEntry<T>.ApplyReturning(items[i]!, ids[i]);
                }
            }
            else
            {
                q.ExecuteSync(con, tx, items.Length == 1);
            }
        }

        public async Task<T> FirstOrDefault<T>(DbCommand cmd, bool prepare = true) where T : new()
        {
            if (AssignmentCache<T>.Constructor == null)
            {
                AssignmentCache<T>.Constructor = GenerateConstructor<T>();
            }

            if (prepare)
            {
                await cmd.PrepareAsync();
            }

            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                if (AssignmentCache<T>.Action == null)
                {
                    AssignmentCache<T>.Action = GenerateAssignment<T>(DefaultNameResolver, reader);
                }

                if (!reader.Read())
                    return default!;

                var item = AssignmentCache<T>.Constructor();
                AssignmentCache<T>.Action(reader, item);

                while (reader.Read()) { }
                return item;
            }
        }
        
        public T FirstOrDefaultSync<T>(DbCommand cmd, bool prepare = true) where T : new()
        {
            if (AssignmentCache<T>.Constructor == null)
            {
                AssignmentCache<T>.Constructor = GenerateConstructor<T>();
            }

            if (prepare)
            {
                cmd.Prepare();
            }

            using (var reader = cmd.ExecuteReader())
            {
                if (AssignmentCache<T>.Action == null)
                {
                    AssignmentCache<T>.Action = GenerateAssignment<T>(DefaultNameResolver, reader);
                }

                if (!reader.Read())
                    return default!;

                var item = AssignmentCache<T>.Constructor();
                AssignmentCache<T>.Action(reader, item);

                while (reader.Read()) { }
                return item;
            }
        }

        public async Task<List<T>> ToList<T>(BaseSelectQuery query, DbConnection con, DbTransaction? tx = null, bool prepare = true) where T : new()
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return await ToList<T>(cmd, prepare).ConfigureAwait(false);
            }
        }
        
        public List<T> ToListSync<T>(BaseSelectQuery query, DbConnection con, DbTransaction? tx = null, bool prepare = true) where T : new()
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return ToListSync<T>(cmd, prepare);
            }
        }

        public async Task<T> FirstOrDefault<T>(BaseSelectQuery query, DbConnection con, DbTransaction? tx = null, bool prepare = true) where T : new()
        {
            using (var cmd = ToCommand(query, con, tx))
                return await FirstOrDefault<T>(cmd, prepare).ConfigureAwait(false);
        }
        
        public T FirstOrDefaultSync<T>(BaseSelectQuery query, DbConnection con, DbTransaction? tx = null, bool prepare = true) where T : new()
        {
            using (var cmd = ToCommand(query, con, tx))
                return FirstOrDefaultSync<T>(cmd, prepare);
        }

        public async Task<T> FirstOrDefault<T>(BaseUpdateQuery query, DbConnection con, DbTransaction? tx = null, bool prepare = true) where T : new()
        {
            using (var cmd = ToCommand(query, con, tx))
                return await FirstOrDefault<T>(cmd, prepare).ConfigureAwait(false);
        }

        public async Task<T> ScalarResult<T>(BaseSelectQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return await ScalarResult<T>(cmd, prepare);
            }
        }
        
        public T ScalarResultSync<T>(BaseSelectQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return ScalarResultSync<T>(cmd, prepare);
            }
        }

        public async Task<T> ScalarResult<T>(BaseUpdateQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return await ScalarResult<T>(cmd, prepare);
            }
        }
        
        public T ScalarResultSync<T>(BaseUpdateQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return ScalarResultSync<T>(cmd, prepare);
            }
        }

        public async Task<T> ScalarResult<T>(BaseInsertQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return await ScalarResult<T>(cmd, prepare);
            }
        }
        
        public T ScalarResultSync<T>(BaseInsertQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return ScalarResultSync<T>(cmd, prepare);
            }
        }

        public async Task<T> ScalarResult<T>(BaseDeleteQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return await ScalarResult<T>(cmd, prepare);
            }
        }
        
        public T ScalarResultSync<T>(BaseDeleteQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return ScalarResultSync<T>(cmd, prepare);
            }
        }

        protected async Task<T> ScalarResult<T>(DbCommand cmd, bool prepare = true)
        {
            if (prepare)
            {
                await cmd.PrepareAsync();
            }

            var val = await cmd.ExecuteScalarAsync().ConfigureAwait(false);

            if (val == DBNull.Value)
                return default!;

            if (val is T variable)
                return variable;

            if (typeof(T) == typeof(long))
                return (T) (object) Convert.ToInt64(val);

            if (typeof(T) == typeof(int))
                return (T) (object) Convert.ToInt32(val);

            if (typeof(T) == typeof(short))
                return (T) (object) Convert.ToInt16(val);
            
            throw new ArgumentException();
        }
        
        protected T ScalarResultSync<T>(DbCommand cmd, bool prepare = true)
        {
            if (prepare)
            {
                cmd.Prepare();
            }

            var val = cmd.ExecuteScalar();

            if (val == DBNull.Value)
                return default!;

            if (val is T variable)
                return variable;

            if (typeof(T) == typeof(long))
                return (T) (object) Convert.ToInt64(val);

            if (typeof(T) == typeof(int))
                return (T) (object) Convert.ToInt32(val);

            if (typeof(T) == typeof(short))
                return (T) (object) Convert.ToInt16(val);
            
            throw new ArgumentException();
        }

        public async Task<List<T>> ScalarListResult<T>(BaseSelectQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return await ScalarListResult<T>(cmd, prepare);
            }
        }
        
        public List<T> ScalarListResultSync<T>(BaseSelectQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return ScalarListResultSync<T>(cmd, prepare);
            }
        }

        public async Task<List<T>> ScalarListResult<T>(BaseUpdateQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return await ScalarListResult<T>(cmd, prepare);
            }
        }
        
        public List<T> ScalarListResultSync<T>(BaseUpdateQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return ScalarListResultSync<T>(cmd, prepare);
            }
        }

        public async Task<List<T>> ScalarListResult<T>(BaseInsertQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return await ScalarListResult<T>(cmd, prepare);
            }
        }
        
        public List<T> ScalarListResultSync<T>(BaseInsertQuery query, DbConnection con, DbTransaction? tx, bool prepare = true)
        {
            using (var cmd = ToCommand(query, con, tx))
            {
                return ScalarListResultSync<T>(cmd, prepare);
            }
        }

        protected async Task<List<T>> ScalarListResult<T>(DbCommand cmd, bool prepare = true)
        {
            var result = new List<T>();

            if (prepare)
            {
                await cmd.PrepareAsync();
            }

            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                while (reader.Read())
                {
                    var val = reader[0];

                    if (val == DBNull.Value)
                        result.Add(default!);
                    else
                    {
                        if (val is T val1)
                        {
                            result.Add(val1);
                        }
                        else if (typeof(T) == typeof(long))
                        {
                            result.Add((T) (object) Convert.ToInt64(val));
                        }
                        else if (typeof(T) == typeof(int))
                        {
                            result.Add((T) (object) Convert.ToInt32(val));
                        }
                        else if (typeof(T) == typeof(short))
                        {
                            result.Add((T) (object) Convert.ToInt16(val));
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                    }
                }
            }

            return result;
        }
        
        protected List<T>  ScalarListResultSync<T>(DbCommand cmd, bool prepare = true)
        {
            var result = new List<T>();

            if (prepare)
            {
                cmd.Prepare();
            }

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var val = reader[0];

                    if (val == DBNull.Value)
                        result.Add(default!);
                    else
                    {
                        if (val is T val1)
                        {
                            result.Add(val1);
                        }
                        else if (typeof(T) == typeof(long))
                        {
                            result.Add((T) (object) Convert.ToInt64(val));
                        }
                        else if (typeof(T) == typeof(int))
                        {
                            result.Add((T) (object) Convert.ToInt32(val));
                        }
                        else if (typeof(T) == typeof(short))
                        {
                            result.Add((T) (object) Convert.ToInt16(val));
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                    }
                }
            }

            return result;
        }
    }
}