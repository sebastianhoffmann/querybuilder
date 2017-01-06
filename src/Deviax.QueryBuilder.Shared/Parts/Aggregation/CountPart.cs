using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public class CountPart : Part
    {
        public IPart Over;

        public CountPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}