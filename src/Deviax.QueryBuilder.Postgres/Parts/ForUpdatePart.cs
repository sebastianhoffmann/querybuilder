using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public class ForUpdatePart : Part
    {
        public readonly ForUpdateWaitPolicy WaitPolicy;

        public ForUpdatePart(ForUpdateWaitPolicy waitPolicy)
        {
            WaitPolicy = waitPolicy;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}
