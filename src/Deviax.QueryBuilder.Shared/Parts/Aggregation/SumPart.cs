using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public class SumPart : Part
    {
        public IPart Over;

        public SumPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}