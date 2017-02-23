using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public partial class CoalescePart : Part
    {
        internal readonly IPart[] Over;

        public CoalescePart(IPart[] over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}