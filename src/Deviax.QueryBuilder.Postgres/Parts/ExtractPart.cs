using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class ExtractPart : Part
    {
        public readonly IPart From;
        public readonly Extractable What;

        public ExtractPart(Extractable what, IPart from)
        {
            What = what;
            From = from;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}