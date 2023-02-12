using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;
using System.Linq;

namespace Deviax.QueryBuilder
{
    public abstract class BaseDeleteQuery : Part
    {
        internal readonly IFromPart From;
        internal readonly IFromPart? UsingPart; // TODO: move to postgres only
        internal readonly List<IBooleanPart>? WhereParts;
        internal readonly List<IPart>? ExtraParameters;

        protected BaseDeleteQuery(
            IFromPart from,
            IFromPart? @using,
            List<IBooleanPart>? whereParts,
            List<IPart>? extraParameters
            )
        {
            From = from;
            UsingPart = @using;
            WhereParts = whereParts;
            ExtraParameters = extraParameters;
        }

        protected BaseDeleteQuery(IFromPart target)
        {
            From = target;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public async Task<T> ScalarResult<T>(DbConnection con, DbTransaction? tx = null, bool prepare = true)
        {
            return await QueryExecutor.DefaultExecutor.ScalarResult<T>(this, con, tx, prepare).ConfigureAwait(false);
        }
        
        public T ScalarResultSync<T>(DbConnection con, DbTransaction? tx = null, bool prepare = true)
        {
            return QueryExecutor.DefaultExecutor.ScalarResultSync<T>(this, con, tx, prepare);
        }

        public async Task<int> Execute(DbConnection con, DbTransaction? tx = null, bool prepare = true)
        {
            return await ScalarResult<int>(con, tx, prepare);
        }
        
        public int ExecuteSync(DbConnection con, DbTransaction? tx = null, bool prepare = true)
        {
            return ScalarResultSync<int>(con, tx, prepare);
        }

        public string StringRepresentation => ToString();

        public override string ToString()
        {
            return QueryExecutor.DefaultExecutor.ToQueryText(this);
        }
    }

    public abstract class BaseDeleteQuery<TQ> : BaseDeleteQuery where TQ : BaseDeleteQuery<TQ>
    {
        protected BaseDeleteQuery(
            IFromPart from,
            IFromPart? @using,
            List<IBooleanPart>? whereParts,
            List<IPart>? extraParameters
            ) : base(from, @using, whereParts, extraParameters)
        {
        }

        protected abstract TQ New(
            TQ t,
            IFromPart from,
            IFromPart? @using,
            List<IBooleanPart>? whereParts,
            List<IPart>? extraParameters
        );

        protected BaseDeleteQuery(IFromPart target) : base(target)
        {
        }

        protected static List<T> With<T>(IReadOnlyCollection<T>? existing, params T[] toAdd)
        {
            if (existing == null)
                return new List<T>(toAdd);

            var l = new List<T>(existing.Count + toAdd.Length);
            l.AddRange(existing);
            l.AddRange(toAdd);
            return l;
        }

        [Pure]
        public TQ Where(params IBooleanPart[] parts)
        {
            return parts.Length == 0 ? (TQ)this : New((TQ)this,
                From, UsingPart, With(WhereParts, parts), ExtraParameters
            );
        }
        
        [Pure]
        public TQ WithExtraParameter(params IPart[] parameters) => New((TQ)this,
             From, UsingPart, WhereParts,
            With(ExtraParameters, parameters)
        );
    }

    public class DeleteQuery<TTable> : BaseDeleteQuery<DeleteQuery<TTable>> where TTable : Table
    {
        internal readonly TTable Table1;
        public DeleteQuery(TTable t)
            : base(t)
        {
            Table1 = t;
        }

        internal DeleteQuery(
            TTable t1,
            IFromPart from,
            IFromPart? @using,
            List<IBooleanPart>? whereParts,
            List<IPart>? extraParameters
            ) : base(from, @using, whereParts, extraParameters)
        {
            Table1 = t1;
        }

        [Pure]
        public DeleteQuery<TTable> Where(params Func<TTable, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new DeleteQuery<TTable>(Table1,
                From, UsingPart, With(WhereParts, parts.Select(p => p(Table1)).ToArray()),
                ExtraParameters
            );
        }
        
        [Pure]
        public DeleteQuery<TTable, TTable2> Using<TTable2>(TTable2 t2) where TTable2 : Table
        {
            return new DeleteQuery<TTable, TTable2>(Table1, t2, From, t2, WhereParts, ExtraParameters);
        }
        
        protected override DeleteQuery<TTable> New(DeleteQuery<TTable> t, IFromPart @from, IFromPart? @using, List<IBooleanPart>? whereParts, List<IPart>? extraParameters)
        {
            return new DeleteQuery<TTable>(t.Table1, from, @using, whereParts, extraParameters);
        }
    }
    
    public class DeleteQuery<TTable, TTable2> : BaseDeleteQuery<DeleteQuery<TTable, TTable2>> where TTable : Table where TTable2 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        public DeleteQuery(TTable t, TTable2 t2)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
        }

        internal DeleteQuery(
            TTable t1,
            TTable2 t2,
            IFromPart from,
            IFromPart? @using,
            List<IBooleanPart>? whereParts,
            List<IPart>? extraParameters
        ) : base(from, @using, whereParts, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
        }

        [Pure]
        public DeleteQuery<TTable, TTable2> Where(params Func<TTable, TTable2, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new DeleteQuery<TTable, TTable2>(Table1, Table2,
                From, UsingPart, With(WhereParts, parts.Select(p => p(Table1, Table2)).ToArray()),
                ExtraParameters
            );
        }
        
        protected override DeleteQuery<TTable, TTable2> New(DeleteQuery<TTable, TTable2> t, IFromPart from, IFromPart? @using, List<IBooleanPart>? whereParts, List<IPart>? extraParameters)
        {
            return new DeleteQuery<TTable, TTable2>(t.Table1, t.Table2, from, @using, whereParts, extraParameters);
        }
    }
}