using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder
{

    public static class N
    {
        public static string Db(string csharpName, bool nullable = false) => QueryExecutor.NameResolver.CSharpToDb(csharpName, nullable);
    }

    public static partial class Q
    {
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

    public class CasePart : IPart
    {
        public IPart ElsePart;

        public List<Tuple<IBooleanPart, IPart>> Whens;

        public CasePart(CasePart casePart, IBooleanPart when, IPart then)
        {
            Whens = new List<Tuple<IBooleanPart, IPart>>(casePart.Whens) {Tuple.Create(when, then)};
            ElsePart = casePart.ElsePart;
        }

        public CasePart(CasePart casePart, IPart elsePart)
        {
            Whens = new List<Tuple<IBooleanPart, IPart>>(casePart.Whens);
            ElsePart = elsePart;
        }

        public CasePart()
        {
            Whens = new List<Tuple<IBooleanPart, IPart>>();
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        [Pure]
        public WhenBuilder When(IBooleanPart booleanPart) => new WhenBuilder(this, booleanPart);

        [Pure]
        public CasePart Else(IPart part) => new CasePart(this, part);

        [Pure]
        public IAliasPart As(string name) => new AliasPart(name, this);

        [Pure]
        public AscOrdering Asc(Nulls n = Nulls.Unspecified) => new AscOrdering(this, n);

        [Pure]
        public AscOrdering Desc(Nulls n = Nulls.Unspecified) => new AscOrdering(this, n);

        public class WhenBuilder
        {
            private readonly IBooleanPart _condition;
            private readonly CasePart _part;

            public WhenBuilder(CasePart part, IBooleanPart condition)
            {
                _part = part;
                _condition = condition;
            }

            [Pure]
            public CasePart Then(IPart part)
            {
                return new CasePart(_part, _condition, part);
            }
        }
    }
}