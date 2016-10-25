using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Deviax.QueryBuilder.Parts;
using System.Reflection;

namespace Deviax.QueryBuilder
{
    public class BaseInsertQuery
    {
        internal readonly Table Target;
        internal readonly List<ValuesCollection> Values;

        internal readonly List<IPart> ReturningParts;

        protected BaseInsertQuery(Table target, List<ValuesCollection> with, List<IPart> returningParts )
        {
            Target = target;
            Values = with;
            ReturningParts = returningParts;
        }

        public BaseInsertQuery(Table target)
        {
            Target = target;
            Values = new List<ValuesCollection>();
            ReturningParts = new List<IPart>();
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
        public BaseInsertQuery Returning(params IPart[] parts)
        {
            return parts.Length == 0 ? this : new BaseInsertQuery(Target, Values, With(ReturningParts, parts));
        }

        public BaseInsertQuery WithValues(params ValuesCollection[] values) => new BaseInsertQuery(Target, With(Values, values), ReturningParts);

        public async Task<T> ScalarResult<T>(DbConnection con, DbTransaction tx = null)
        {
            return await QueryExecutor.DefaultExecutor.ScalarResult<T>(this, con, tx).ConfigureAwait(false);
        }

        public async Task<List<T>> ScalarList<T>(DbConnection con, DbTransaction tx = null)
        {
            return await QueryExecutor.DefaultExecutor.ScalarListResult<T>(this, con, tx).ConfigureAwait(false);
        }

        public async Task<int> Execute(DbConnection con, DbTransaction tx = null)
        {
            return await QueryExecutor.DefaultExecutor.ScalarResult<int>(this, con, tx).ConfigureAwait(false);
        }
    }

    public abstract class BaseInsertQuery<TQ> : BaseInsertQuery where TQ : BaseInsertQuery<TQ>
    {
        protected BaseInsertQuery(Table target, List<ValuesCollection> with, List<IPart> returningParts) : base(target, with, returningParts)
        {
        }

        [Pure]
        public new TQ Returning(params IPart[] parts)
        {
            return parts.Length == 0 ? (TQ)this : New(Target, Values, With(ReturningParts, parts));
        }

        public new TQ WithValues(params ValuesCollection[] values) => New(Target, With(Values, values), ReturningParts);
        protected abstract TQ New(Table target, List<ValuesCollection> with, List<IPart> returningParts);
    }

    public class InsertQuery<TTable> : BaseInsertQuery<InsertQuery<TTable>> where TTable : Table
    {
        private TTable _target;

        public InsertQuery(TTable target, List<ValuesCollection> with, List<IPart> returningParts) : base(target, with, returningParts)
        {
            _target = target;
        }

        public InsertQuery(TTable target) : base(target, new List<ValuesCollection>(), new List<IPart>())
        {
            _target = target;
        }
        public ValuesCollection<TTable, InsertQuery<TTable>> NewValues => new ValuesCollection<TTable, InsertQuery<TTable>>(_target, this);
        protected override InsertQuery<TTable> New(Table target, List<ValuesCollection> with, List<IPart> returningParts)
        {
            return new InsertQuery<TTable>((TTable)target, with, returningParts);
        }
    }

    public class ValuesCollection
    {
        protected static List<T> With<T>(IReadOnlyCollection<T> existing, params T[] toAdd)
        {
            if (existing == null)
                return new List<T>(toAdd);

            var l = new List<T>(existing.Count + toAdd.Length);
            l.AddRange(existing);
            l.AddRange(toAdd);
            return l;
        }

        private BaseInsertQuery _query;
        internal readonly List<SetFieldPart> Values;

        protected ValuesCollection()
        {
            Values = new List<SetFieldPart>();
        }

        protected ValuesCollection(BaseInsertQuery query, List<SetFieldPart> values)
        {
            _query = query;
            Values = values;
        }

        public ValuesCollection Add(params SetFieldPart[] parts) => new ValuesCollection(_query, With(Values, parts));

        public BaseInsertQuery EndValues()
        {
            var q = _query;
            _query = null;
            return q.WithValues(this);

        }
    }

    public class ValuesCollection<TTable, TQ> : ValuesCollection where TTable : Table where TQ : BaseInsertQuery<TQ>
    {
        private TTable _table;
        private TQ _query;

        public ValuesCollection(TTable table, TQ query)
        {
            _table = table;
            _query = query;
        }

        public ValuesCollection(TTable table, TQ query, List<SetFieldPart> values ) : base(query, values)
        {
            _table = table;
            _query = query;
        }

        public new TQ EndValues()
        {
            var q = _query;
            _query = null;
            _table = null;
            return q.WithValues(this);

        }
        public new ValuesCollection<TTable, TQ> Add(params SetFieldPart[] parts) => new ValuesCollection<TTable, TQ>(_table, _query, With(Values, parts));
        public ValuesCollection<TTable, TQ> Add(params Func<TTable, SetFieldPart>[] f)
            => new ValuesCollection<TTable, TQ>(_table, _query, With(Values, f.Select(a => a(_table)).ToArray()));
    }
}