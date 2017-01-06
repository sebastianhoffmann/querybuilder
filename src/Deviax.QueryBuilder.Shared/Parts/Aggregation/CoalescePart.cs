using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public class CoalescePart : Part
    {
        public IPart[] Over;

        public CoalescePart(IPart[] over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}