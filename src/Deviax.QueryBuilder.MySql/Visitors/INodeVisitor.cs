using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public partial interface INodeVisitor
    {
        void Visit(BaseUpdateQuery updateQuery);
        void Visit(CastPart castPart);
    }
}