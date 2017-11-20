using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Parts.Aggregation;
using Deviax.QueryBuilder.Parts.FullTextSearch;

namespace Deviax.QueryBuilder.Visitors
{
    public partial interface INodeVisitor
    {
        void Visit(ArrayAggPart arrayAggPart);
        void Visit(ToTsVectorPart toTsVector);
        void Visit(ToTsQueryPart toTsQuery);
        void Visit(FullTextSearchMatchPart fullTextSearchMatch);
        void Visit(BaseUpdateQuery updateQuery);
        void Visit(AgePart agePart);
        void Visit(ExtractPart extractPart);
        void Visit(CastPart castPart);
        void Visit(RowNumberPart rowNumberPart);
        void Visit(PartitionPart partitionPart);
        void Visit(ContainsPart containsPart);
        void Visit(MatchesRegexPart matchesRegexPart);
        void Visit(UnnestTable unnestTable);
        void Visit(ArrayOverlapPart arrayOverlapPart);
    }
}