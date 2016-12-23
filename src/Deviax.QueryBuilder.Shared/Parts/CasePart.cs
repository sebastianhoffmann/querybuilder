using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class CasePart : IPart
    {
        public IPart ElsePart;

        public List<Tuple<IBooleanPart, IPart>> Whens;

        public CasePart(CasePart casePart, IBooleanPart when, IPart then)
        {
            Whens = new List<Tuple<IBooleanPart, IPart>>(casePart.Whens) { Tuple.Create(when, then) };
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
        public DescOrdering Desc(Nulls n = Nulls.Unspecified) => new DescOrdering(this, n);

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
