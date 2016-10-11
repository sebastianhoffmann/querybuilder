using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class IsNullPart : Part, IBooleanPart
    {
        public readonly IPart Part;

        public IsNullPart(IPart part)
        {
            Part = part;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class IsNotNullPart : Part, IBooleanPart
    {
        internal readonly IPart Part;

        public IsNotNullPart(IPart part)
        {
            Part = part;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}