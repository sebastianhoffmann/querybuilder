using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class AgePart : Part
    {
        public readonly IPart Of;

        public AgePart(IPart of)
        {
            Of = of;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}