﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;
using System.Linq;

namespace Deviax.QueryBuilder
{
    public abstract class BaseUpdateQuery : Part
    {
        internal readonly IFromPart Target;
        internal readonly IFromPart? FromPart; // TODO: move to postgres
        internal readonly List<IBooleanPart>? WhereParts;
        internal readonly List<IPart>? ReturningParts;
        internal readonly List<IPart>? ExtraParameters;
        internal readonly List<ISetPart>? SetParts;

        protected BaseUpdateQuery(
            IFromPart target,
            IFromPart? from,
            List<IBooleanPart>? whereParts,
            List<IPart>? returningParts,
            List<IPart>? extraParameters,
            List<ISetPart>? setParts
            )
        {
            Target = target;
            FromPart = from;
            WhereParts = whereParts;
            ReturningParts = returningParts;
            ExtraParameters = extraParameters;
            SetParts = setParts;
        }

        protected BaseUpdateQuery(IFromPart target)
        {
            Target = target;
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

    public abstract class BaseUpdateQuery<TQ> : BaseUpdateQuery where TQ : BaseUpdateQuery<TQ>
    {
        protected BaseUpdateQuery(
            IFromPart target,
            IFromPart? from,
            List<IBooleanPart>? whereParts,
            List<IPart>? returningParts,
            List<IPart>? extraParameters,
            List<ISetPart>? setParts
            ) : base(target, from, whereParts, returningParts, extraParameters, setParts)
        {
        }

        protected abstract TQ New(
            TQ t,
            IFromPart target,
            IFromPart? from,
            List<IBooleanPart>? whereParts,
            List<IPart>? returningParts,
            List<IPart>? extraParameters,
            List<ISetPart>? setParts
        );

        protected BaseUpdateQuery(IFromPart target) : base(target)
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
                Target,FromPart, With(WhereParts, parts), ReturningParts,
                ExtraParameters, SetParts
            );
        }

        [Pure]
        public TQ Set(params ISetPart[] parts)
        {
            return parts.Length == 0 ? (TQ)this : New((TQ)this,
                Target, FromPart, WhereParts, ReturningParts,
                ExtraParameters, With(SetParts, parts)
            );
        }

        [Pure]
        public TQ Returning(params IPart[] parts)
        {
            return parts.Length == 0 ? (TQ)this : New((TQ)this,
                Target, FromPart, WhereParts, With(ReturningParts,parts),
                ExtraParameters,SetParts
            );
        }

        [Pure]
        public TQ WithExtraParameter(params IPart[] parameters) => New((TQ)this,
             Target, FromPart, WhereParts, ReturningParts,
            With(ExtraParameters, parameters), SetParts
        );
    }

    public class UpdateQuery<TTable> : BaseUpdateQuery<UpdateQuery<TTable>> where TTable : Table
    {
        internal readonly TTable Table1;
        public UpdateQuery(TTable t)
            : base(t)
        {
            Table1 = t;
        }

        internal UpdateQuery(
            TTable t1,
            IFromPart target,
            IFromPart? from,
            List<IBooleanPart>? whereParts,
            List<IPart>? returningParts,
            List<IPart>? extraParameters,
            List<ISetPart>? setParts
            ) : base(target, from, whereParts, returningParts, extraParameters, setParts)
        {
            Table1 = t1;
        }

        [Pure]
        public UpdateQuery<TTable> Where(params Func<TTable, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new UpdateQuery<TTable>(Table1,
                Target, FromPart, With(WhereParts, parts.Select(p => p(Table1)).ToArray()), 
                ReturningParts,ExtraParameters, SetParts
            );
        }
        
        [Pure]
        public UpdateQuery<TTable, TTable2> From<TTable2>(TTable2 t2) where TTable2 : Table
        {
            return new UpdateQuery<TTable, TTable2>(Table1, t2,
                Target, t2, WhereParts, 
                ReturningParts,ExtraParameters, SetParts
            );
        }

        [Pure]
        public UpdateQuery<TTable> Set(params Func<TTable, ISetPart>[] parts)
        {
            return parts.Length == 0 ? this : new UpdateQuery<TTable>(Table1,
                Target, FromPart, WhereParts,
                ReturningParts, ExtraParameters, With(SetParts, parts.Select(p => p(Table1)).ToArray())
            );
        }

        protected override UpdateQuery<TTable> New(UpdateQuery<TTable> t, IFromPart target, IFromPart? from, List<IBooleanPart>? whereParts, List<IPart>? returningParts, List<IPart>? extraParameters, List<ISetPart>? setParts)
        {
           return new UpdateQuery<TTable>(t.Table1, target, from, whereParts, returningParts, extraParameters, setParts);
        }
    }
    
    public class UpdateQuery<TTable, TTable2> : BaseUpdateQuery<UpdateQuery<TTable, TTable2>> where TTable : Table where TTable2 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        
        internal UpdateQuery(
            TTable t1,
            TTable2 t2,
            IFromPart target,
            IFromPart? from,
            List<IBooleanPart>? whereParts,
            List<IPart>? returningParts,
            List<IPart>? extraParameters,
            List<ISetPart>? setParts
            ) : base(target, from, whereParts, returningParts, extraParameters, setParts)
        {
            Table1 = t1;
            Table2 = t2;
        }

        [Pure]
        public UpdateQuery<TTable, TTable2> Where(params Func<TTable, TTable2, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new UpdateQuery<TTable, TTable2>(Table1, Table2,
                Target, FromPart, With(WhereParts, parts.Select(p => p(Table1, Table2)).ToArray()), 
                ReturningParts,ExtraParameters, SetParts
            );
        }

        [Pure]
        public UpdateQuery<TTable, TTable2> Set(params Func<TTable, TTable2, ISetPart>[] parts)
        {
            return parts.Length == 0 ? this : new UpdateQuery<TTable, TTable2>(Table1, Table2,
                Target, FromPart, WhereParts,
                ReturningParts, ExtraParameters, With(SetParts, parts.Select(p => p(Table1, Table2)).ToArray())
            );
        }

        protected override UpdateQuery<TTable, TTable2> New(UpdateQuery<TTable, TTable2> t, IFromPart target, IFromPart? from, List<IBooleanPart>? whereParts, List<IPart>? returningParts, List<IPart>? extraParameters, List<ISetPart>? setParts)
        {
           return new UpdateQuery<TTable, TTable2>(t.Table1, t.Table2, target, from, whereParts, returningParts, extraParameters, setParts);
        }
    }


    public class UpdateQuery : BaseUpdateQuery<UpdateQuery>
    {
        internal UpdateQuery(
            IFromPart target,
            IFromPart? from,
            List<IBooleanPart>? whereParts,
            List<IPart>? returningParts,
            List<IPart>? extraParameters,
            List<ISetPart>? setParts
            ) : base(target, from, whereParts, returningParts, extraParameters, setParts) { }

        public UpdateQuery(IFromPart target) : base(target)
        {

        }

        public UpdateQuery From(IFromPart f) => New(this, Target, f, WhereParts, ReturningParts, ExtraParameters, SetParts);
        
        protected override UpdateQuery New(
            UpdateQuery t,
            IFromPart target, IFromPart? from, List<IBooleanPart>? whereParts, List<IPart>? returningParts, List<IPart>? extraParameters, List<ISetPart>? setParts
        ) => new UpdateQuery(target, from, whereParts, returningParts, extraParameters, setParts);
    }
}