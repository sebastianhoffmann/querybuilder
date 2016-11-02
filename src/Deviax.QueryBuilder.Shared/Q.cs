using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Parts;
using System.Data.Common;
using System.Threading.Tasks;

namespace Deviax.QueryBuilder
{
    public static class N
    {
        public static string Db(string csharpName, bool nullable = false) => QueryExecutor.NameResolver.CSharpToDb(csharpName, nullable);
    }

    public static partial class Q
    {
        [Pure]
        public static InsertQuery<TTable> InsertInto<TTable>(TTable table) where TTable : Table => new InsertQuery<TTable>(table);

        public static async Task Insert<T>(DbConnection con, DbTransaction tx, params T[] items)
        {
            await QueryExecutor.DefaultExecutor.Insert(items, con, tx);
        }

        public static async Task Insert<T>(DbConnection con, params T[] items)
        {
            await QueryExecutor.DefaultExecutor.Insert(items, con, null);
        }

        [Pure]
        public static DeleteQuery<TTable> DeleteFrom<TTable>(TTable table) where TTable : Table => new DeleteQuery<TTable>(table);

        [Pure]
        public static CasePart Case => new CasePart();

        [Pure]
        public static Literal<T> L<T>(T t) => new Literal<T>(t);

        [Pure]
        public static Literal<T> Literal<T>(T literal) => L(literal);

        [Pure]
        public static UpdateQuery Update(IFromPart p) => new UpdateQuery(p);

        [Pure]
        public static UpdateQuery<T> Update<T>(T p) where T : Table  => new UpdateQuery<T>(p) ;

        [Pure]
        public static SelectQuery From(IFromPart p) => new SelectQuery(p);
        [Pure]
        public static SelectQuery<T> From<T>(T p) where T : Table => new SelectQuery<T>(p) ;

        [Pure]
        public static IntervalPart Interval(IPart n, IntervalType t) => new IntervalPart(n, t);

        [Pure]
        public static Parameter<T> P<T>(string name, T value) => new Parameter<T>(value, name);

        [Pure]
        public static Parameter<T> Parameter<T>(string name, T value) => P(name, value);

        [Pure]
        public static ArrayParameter<T> P<T>(string name, IEnumerable<T> value) => new ArrayParameter<T>(value, name);

        [Pure]
        public static ArrayParameter<T> AP<T>(string name, params T[] values) => P<T>(name, values);

        [Pure]
        public static ArrayParameter<T> ArrayParameter<T>(string name, params T[] values) => P<T>(name, values);

        [Pure]
        public static MinPart Min(IPart over) => new MinPart(over);

        [Pure]
        public static MaxPart Max(IPart over) => new MaxPart(over);

        [Pure]
        public static LowerPart Lower(IPart over) => new LowerPart(over);

        [Pure]
        public static SumPart Sum(IPart over) => new SumPart(over);

        [Pure]
        public static CountPart Count(IPart over) => new CountPart(over);

        [Pure]
        public static CoalescePart Coalesce(params IPart[] over) => new CoalescePart(over);

        [Pure]
        public static DistinctPart Distinct(params IPart[] over) => new DistinctPart(over);
    }
}