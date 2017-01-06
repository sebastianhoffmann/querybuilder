using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public class MaxPart : Part
    {
        public IPart Over;

        public MaxPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}