using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class MinPart : Part
    {
        public IPart Over;
        public MinPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class SumPart : Part
    {
        public IPart Over;
        public SumPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class CountPart : Part
    {
        public IPart Over;
        public CountPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class CoalescePart : Part
    {
        public IPart[] Over;
        public CoalescePart(IPart[] over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class DistinctPart : Part
    {
        public IPart[] Over;
        public DistinctPart(IPart[] over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class MaxPart : Part
    {
        public IPart Over;
        public MaxPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}