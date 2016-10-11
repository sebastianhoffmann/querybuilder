using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public interface IOrderPart
    {
        void Accept(INodeVisitor visitor);
    }

    public class AscOrdering : IOrderPart
    {
        public readonly IPart Part;
        public readonly Nulls Nulls;

        public AscOrdering(IPart part, Nulls nulls)
        {
            Part = part;
            Nulls = nulls;
        }

        public void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class DescOrdering : IOrderPart
    {
        public readonly IPart Part;
        public readonly Nulls Nulls;

        public DescOrdering(IPart part, Nulls nulls)
        {
            Part = part;
            Nulls = nulls;
        }

        public void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public enum Nulls
    {
        Unspecified,
        First,
        Last
    }
}