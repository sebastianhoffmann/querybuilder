using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public partial interface INodeVisitor
    {
        void Visit(ArrayAggPart arrayAggPart);
        void Visit(ToTsVectorPart toTsVector);
        void Visit(ToTsQueryPart toTsQuery);
        void Visit(FullTextSearchMatchPart fullTextSearchMatch);
        void Visit(BaseUpdateQuery updateQuery);
    }
}