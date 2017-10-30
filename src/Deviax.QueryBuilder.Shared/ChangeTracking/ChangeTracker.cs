using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.ChangeTracking
{
    public class ChangeTrackingContext
    {
        public class Tracking
        {
            public object Original;
            public object Current;
            public Action<Tracking, object, object, List<FieldChange>> Comparer;
            public Table Table;
            public Func<object, Table, IBooleanPart[]> ConditionGetter;
        }
        
        public IEnumerable<Tuple<object, UpdateQuery>> ToUpdateQueries()
        {
            return Collect()
                .GroupBy(c => c.Tracking)
                .Select(group => {
                    var qry = group.Aggregate(new UpdateQuery(group.Key.Table), (q, i) => q.Set(group.Key.Table.F(N.Db(i.Field)).Set(i.Parameter)));

                    if (group.Key.ConditionGetter != null)
                        qry = qry.Where(group.Key.ConditionGetter(group.Key.Original, group.Key.Table));

                    return Tuple.Create(group.Key.Current, qry);
                });
        }

        public async Task<long> Commit(DbConnection con, DbTransaction tx = null)
        {
            var cnt = 0L;
            foreach (var qt in ToUpdateQueries())
            {
                cnt += await qt.Item2.ScalarResult<long>(con, tx);
            }
            return cnt;
        }
        
        public long CommitSync(DbConnection con, DbTransaction tx)
        {
            var cnt = 0L;
            foreach (var qt in ToUpdateQueries())
            {
                cnt += qt.Item2.ScalarResultSync<long>(con, tx);
            }
            return cnt;
        }

        public void Clear()
        {
            _objects.Clear();
        }

        public UpdateQuery ToFirstQuery() => ToUpdateQueries().FirstOrDefault()?.Item2;

        private readonly List<Tracking> _objects = new List<Tracking>();
        public void Track<T>(T current, T copy, Table table = null)
        {
            var comparison = ComparisonCache<T>.Comparison ?? (ComparisonCache<T>.Comparison = BuildComparison<T>());

            table = TypeToTableEntry<T>.DefaultTable;

            if (table == null)
            {
                throw new ArgumentException($"Please register `{typeof(T).FullName}` with a table via Registry.RegisterTypeToTable or pass a Table to Track");
            }

            _objects.Add(new Tracking { Original = copy, Current = current, Comparer = comparison, Table = table, ConditionGetter = TypeToTableEntry<T>.GetDefaultConditions });
        }

        public void Track<T>(T current, Table table = null)
        {
            Track(current, Copy(current), table);
        }

        public static ChangeTrackingContext StartWith<T>(T current, Table table = null)
        {
            var ctc = new ChangeTrackingContext();
            ctc.Track(current, table);
            return ctc;
        }

        private T Copy<T>(T item)
        {
            if (CopyCache<T>.F == null)
            {
                var p1 = Expression.Parameter(typeof(T));
                CopyCache<T>.F = Expression.Lambda<Func<T,T>>(
                Expression.Convert(Expression.Call(p1, typeof(T).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance)), typeof(T)),
                p1).Compile();
            }

            return CopyCache<T>.F(item);
        }

        private static Expression NewChange(Expression tracking, Type t, MemberInfo mi, Expression val)
        {
            var constructor = typeof(FieldChange<>).MakeGenericType(t).GetConstructor(new[] { typeof(Tracking), typeof(string), t });
            return Expression.Convert(Expression.New(constructor, tracking, Expression.Constant(mi.Name), val), typeof(FieldChange));
        }

        private static Action<Tracking, object, object, List<FieldChange>> BuildComparison<T>()
        {
            var t = typeof(T);
            var ti = t.GetTypeInfo();

            var props = ti.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            var fields = ti.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField);

            var ot = typeof(object);

            var o1Arg = Expression.Parameter(ot);
            var o2Arg = Expression.Parameter(ot);

            var changeListType = typeof(List<FieldChange>);

            var addChangeMethod = changeListType.GetMethod("Add", new[] { typeof(FieldChange) });

            var listArg = Expression.Parameter(changeListType);
            var original = Expression.Variable(t);
            var current = Expression.Variable(t);
            var trackingArg = Expression.Parameter(typeof(Tracking));

            var comparers = new List<Expression>();


            var stringType = typeof(string);
            var listType = typeof(List<>);

            foreach (var prop in props)
            {
                var originalVal = Expression.Property(original, prop);
                var currentVal = Expression.Property(current, prop);
                
                if (prop.PropertyType.IsByRef)
                {
                    if (prop.PropertyType.IsConstructedGenericType)
                    {
                        if (prop.PropertyType.GetGenericTypeDefinition() == listType)
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else if (prop.PropertyType == stringType)
                    {
                        // Todo: special case?
                        comparers.Add(
                            Expression.IfThen(
                                Expression.Not(Expression.Equal(originalVal, currentVal)),
                                Expression.Call(listArg, addChangeMethod, NewChange(trackingArg, prop.PropertyType, prop, currentVal))
                            )
                        );
                    }
                }
                else
                {
                    comparers.Add(
                        Expression.IfThen(Expression.Not(Expression.Equal(originalVal, currentVal)),
                            Expression.Call(listArg, addChangeMethod, NewChange(trackingArg, prop.PropertyType, prop, currentVal))
                        )
                    );
                }
            }

            foreach (var f in fields)
            {
                var originalVal = Expression.Field(original, f);
                var currentVal = Expression.Field(current, f);


                if (f.FieldType.IsByRef)
                {
                    if (f.FieldType.IsConstructedGenericType)
                    {
                        if (f.FieldType.GetGenericTypeDefinition() == listType)
                        {
                            throw new NotImplementedException();
                        }
                    }
                    else if (f.FieldType == stringType)
                    {
                        // Todo: special case?
                        comparers.Add(
                            Expression.IfThen(
                                Expression.Not(Expression.Equal(originalVal, currentVal)),
                                Expression.Call(listArg, addChangeMethod, NewChange(trackingArg, f.FieldType, f, currentVal))
                            )
                        );
                    }
                }
                else
                {
                    comparers.Add(
                        Expression.IfThen(Expression.Not(Expression.Equal(originalVal, currentVal)),
                            Expression.Call(listArg, addChangeMethod, NewChange(trackingArg, f.FieldType, f, currentVal))
                        )
                    );
                }
            }

            var exp = Expression.Lambda<Action<Tracking, object, object, List<FieldChange>>>(
                Expression.Block(new[] { original, current }, new[] {
                    Expression.Assign(original, Expression.Convert(o1Arg, t)),
                    Expression.Assign(current, Expression.Convert(o2Arg, t)),
                }.Concat(comparers).ToArray())
                , new[] { trackingArg, o1Arg, o2Arg, listArg }
            );

            var compiled = exp.Compile();
            return compiled;
        }

        public IEnumerable<FieldChange> Collect()
        {
            var changes = new List<FieldChange>();

            foreach (var tracked in _objects)
                tracked.Comparer(tracked, tracked.Original, tracked.Current, changes);

            return changes;
        }

        internal static class ComparisonCache<T>
        {
            public static Action<Tracking, object, object, List<FieldChange>> Comparison;
        }

        public abstract class FieldChange
        {
            public FieldChange(Tracking tracking, string field)
            {
                Tracking = tracking;
                Field = field;
            }
            public abstract IPart Parameter { get; }
            public Tracking Tracking;
            public string Field;
        }

        public class FieldChange<TValue> : FieldChange
        {
            public FieldChange(Tracking tracking, string field, TValue current) : base(tracking, field)
            {
                Current = current;
            }

            public override IPart Parameter { get { return new Parameter<TValue>(Current, Field); } }

            public TValue Current;
        }
    }

    internal static class CopyCache<T>
    {
        public static Func<T, T> F;
    }
}
