using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{

    public class StringConcatenation : Part
    {
        internal readonly IPart Left;
        internal readonly IPart Right;

        public StringConcatenation(IPart left, IPart right)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
    
    public sealed class LikePart : Part, IBooleanPart
    {
        internal readonly IPart Left;
        internal readonly IPart Right;
        internal readonly LikeMode Mode;

        public LikePart(IPart left, IPart right, LikeMode mode)
        {
            Left = left;
            Right = right;
            Mode = mode;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class LowerPart : Part
    {
        public readonly IPart Over;

        public LowerPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public enum LikeMode
    {
        CaseSensitive,
        IgnoreCase,
    }
}