using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class RowNumberPart : Part
    {
        public readonly IPartitionOverPart Over;

        public RowNumberPart(IPartitionOverPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}