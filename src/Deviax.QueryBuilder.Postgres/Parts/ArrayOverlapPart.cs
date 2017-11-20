using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class ArrayOverlapPart: Part, IBooleanPart
    {
        public readonly IPart Left;
        public readonly IPart Right;

        public ArrayOverlapPart(IPart left, IPart right)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}