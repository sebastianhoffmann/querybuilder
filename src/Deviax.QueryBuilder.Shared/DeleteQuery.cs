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
        internal readonly List<IBooleanPart> WhereParts;
        internal readonly List<IPart> ExtraParameters;

        protected BaseDeleteQuery(
            IFromPart from,
            List<IBooleanPart> whereParts,
            List<IPart> extraParameters
            )
        {
           
            From = from;
            WhereParts = whereParts;
            ExtraParameters = extraParameters;
        }

        protected BaseDeleteQuery(IFromPart target)
        {
            From = target;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public async Task<T> ScalarResult<T>(DbConnection con, DbTransaction tx = null)
        {
            return await QueryExecutor.DefaultExecutor.ScalarResult<T>(this, con, tx).ConfigureAwait(false);
        }

        public async Task<int> Execute(DbConnection con, DbTransaction tx = null)
        {
            return await ScalarResult<int>(con, tx);
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
            List<IBooleanPart> whereParts,
            List<IPart> extraParameters
            ) : base(from, whereParts, extraParameters)
        {
        }

        protected abstract TQ New(
            TQ t,
            IFromPart from,
            List<IBooleanPart> whereParts,
            List<IPart> extraParameters
        );

        protected BaseDeleteQuery(IFromPart target) : base(target)
        {
        }

        protected static List<T> With<T>(IReadOnlyCollection<T> existing, params T[] toAdd)
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
                From, With(WhereParts, parts), ExtraParameters
            );
        }
        
        [Pure]
        public TQ WithExtraParameter(params IPart[] parameters) => New((TQ)this,
             From, WhereParts,
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
            List<IBooleanPart> whereParts,
            List<IPart> extraParameters
            ) : base(from, whereParts, extraParameters)
        {
            Table1 = t1;
        }

        [Pure]
        public DeleteQuery<TTable> Where(params Func<TTable, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new DeleteQuery<TTable>(Table1,
                From, With(WhereParts, parts.Select(p => p(Table1)).ToArray()),
                ExtraParameters
            );
        }
        
        protected override DeleteQuery<TTable> New(DeleteQuery<TTable> t, IFromPart @from, List<IBooleanPart> whereParts, List<IPart> extraParameters)
        {
            return new DeleteQuery<TTable>(t.Table1, from, whereParts, extraParameters);
        }
    }
}