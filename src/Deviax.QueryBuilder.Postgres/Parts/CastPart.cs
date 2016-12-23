using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class CastPart : Part
    {
        public readonly IPart What;
        public readonly string TargetType;

        public CastPart(IPart what, string targetType)
        {
            What = what;
            TargetType = targetType;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}