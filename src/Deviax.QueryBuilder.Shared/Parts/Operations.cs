using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class PlusPart : LeftRightPart
    {
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
        public PlusPart(IPart left, IPart right) : base(left, right) { }
    }

    public class MinusPart : LeftRightPart
    {
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
        public MinusPart(IPart left, IPart right) : base(left, right) { }
    }

    public class MulPart : LeftRightPart
    {
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
        public MulPart(IPart left, IPart right) : base(left, right) { }
    }

    public class DivPart : LeftRightPart
    {
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
        public DivPart(IPart left, IPart right) : base(left, right) { }
    }

    public class ModPart : LeftRightPart
    {
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
        public ModPart(IPart left, IPart right) : base(left, right) { }
    }

    public class AbsPart : Part
    {
        public IPart Over;
        public AbsPart(IPart over)
        {
            Over = over;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}