using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public class DistinctPart : Part
    {
        public IPart[] Over;

        public DistinctPart(IPart[] over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}