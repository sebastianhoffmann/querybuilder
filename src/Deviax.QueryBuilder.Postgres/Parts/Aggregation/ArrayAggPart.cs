using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public class ArrayAggPart : Part
    {
        internal readonly IPart Over;
        internal readonly IBooleanPart? FilterPart;

        public ArrayAggPart(IPart over)
        {
            Over = over;
        }

        public ArrayAggPart(IPart over, IBooleanPart filterPart)
        {
            Over = over;
            FilterPart = filterPart;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public ArrayAggPart Filter(IBooleanPart filter)
        {
            return new ArrayAggPart(Over, filter);
        }
    }
}