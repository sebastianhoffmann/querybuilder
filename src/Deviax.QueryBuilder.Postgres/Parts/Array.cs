using System.Reflection;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class ArrayAggPart : Part
    {
        public IPart Over;
        public ArrayAggPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class ContainsPart : Part
    {
        public readonly IPart Left;
        public readonly IPart Right;

        public ContainsPart(IPart left, IPart right)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}