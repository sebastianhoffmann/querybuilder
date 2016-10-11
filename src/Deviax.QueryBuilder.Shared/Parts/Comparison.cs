using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class EqPart : LeftRightPart, IBooleanPart
    {
        public EqPart(IPart left, IPart right) : base(left, right) { }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class NeqPart : LeftRightPart, IBooleanPart
    {
        public NeqPart(IPart left, IPart right) : base(left, right) { }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class LtPart : LeftRightPart, IBooleanPart
    {
        public LtPart(IPart left, IPart right) : base(left, right) { }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class GtPart : LeftRightPart, IBooleanPart
    {
        public GtPart(IPart left, IPart right) : base(left, right) { }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class LtePart : LeftRightPart, IBooleanPart
    {
        public LtePart(IPart left, IPart right) : base(left, right) { }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class GtePart : LeftRightPart, IBooleanPart
    {
        public GtePart(IPart left, IPart right) : base(left, right) { }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class InPart : LeftRightPart, IBooleanPart
    {
        public InPart(IPart left, IPart right) : base(left, right) { }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
    
    public class BetweenPart : Part, IBooleanPart
    {
        public readonly IPart IntFieldPart;
        public readonly IPart Left;
        public readonly IPart Right;

        public BetweenPart(IPart intFieldPart, IPart left, IPart right)
        {
            IntFieldPart = intFieldPart;
            Left = left;
            Right = right;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}