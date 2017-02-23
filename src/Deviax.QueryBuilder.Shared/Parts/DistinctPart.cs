using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public partial class DistinctPart : Part
    {
        internal readonly IPart[] Over;

        public DistinctPart(IPart[] over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}