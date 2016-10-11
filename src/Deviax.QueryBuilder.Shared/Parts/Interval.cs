using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public enum IntervalType
    {
        Day,
        Month,
        Week,
        Year,
        Hour,
        Minute,
        Second
    }

    public class IntervalPart : Part
    {
        public readonly IPart Part;
        public readonly IntervalType IntervalType;

        public IntervalPart(IPart part, IntervalType intervalType)
        {
            Part = part;
            IntervalType = intervalType;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}