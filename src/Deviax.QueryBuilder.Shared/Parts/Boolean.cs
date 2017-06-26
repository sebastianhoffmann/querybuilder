using System.Collections.Generic;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public interface IBooleanPart : IPart { }

    public class IsFalsePart : Part, IBooleanPart
    {
        public readonly IPart Part;
        public IsFalsePart(IPart part) { Part = part; }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class IsTruePart : Part, IBooleanPart
    {
        public readonly IPart Part;
        public IsTruePart(IPart part){Part = part;}
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class AndPart : LeftRightPart, IBooleanPart
    {
        public static IBooleanPart Build(params IBooleanPart[] parts)
        {
            var builder = new Builder();
            foreach (var booleanPart in parts)
            {
                builder.Add(booleanPart);
            }
            return builder.Build();
        }

        public AndPart(IPart left, IPart right) : base(left, right) { }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public class Builder
        {
            private readonly List<IBooleanPart> _parts = new List<IBooleanPart>();

            public Builder Add(IBooleanPart part)
            {
                if (part != null)
                    _parts.Add(part);

                return this;
            }
            
            public T AppendTo<T>(T q) where T : BaseSelectQuery<T> => _parts.Count == 0 ? q : q.Where(Build());

            public IBooleanPart Build()
            {
                if (_parts.Count == 0)
                    return null;

                if (_parts.Count == 1)
                    return _parts[0];

                var part = new AndPart(_parts[0], _parts[1]);

                for (int i = 2; i < _parts.Count; i++)
                {
                    part = new AndPart(part, _parts[i]);
                }

                return part;
            }
        }
    }
    public class OrPart : LeftRightPart, IBooleanPart
    {
        public static IBooleanPart Build(params IBooleanPart[] parts)
        {
            var builder = new Builder();
            foreach (var booleanPart in parts)
            {
                builder.Add(booleanPart);
            }
            return builder.Build();
        }

        public class Builder
        {
            private readonly List<IBooleanPart>  _parts = new List<IBooleanPart>();

            public Builder Add(IBooleanPart part)
            {
                if(part != null)
                    _parts.Add(part);

                return this;
            }

            public T AppendTo<T>(T q) where T : BaseSelectQuery<T> => _parts.Count == 0 ? q : q.Where(Build());

            public IBooleanPart Build()
            {
                if (_parts.Count == 0)
                    return null;

                if (_parts.Count == 1)
                    return _parts[0];

                var part = new OrPart(_parts[0], _parts[1]);

                for (int i = 2; i < _parts.Count; i++)
                {
                    part = new OrPart(part, _parts[i]);
                }

                return part;
            }
        }

        public OrPart(IPart left, IPart right) : base(left, right) {}
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
    
    public class ExistsPart : Part, IBooleanPart
    {
        public IPart What;
        
        public ExistsPart(IPart what)
        {
            What = what;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}