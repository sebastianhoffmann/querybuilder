using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class PartitionPart : Part, IPartitionOverPart
    {
        public readonly IPart Thing;
        public IOrderPart? OrderPart;

        public PartitionPart(IPart thing)
        {
            Thing = thing;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public PartitionPart OrderBy(IOrderPart order)
        {
            OrderPart = order;
            return this;
        }
    }
}