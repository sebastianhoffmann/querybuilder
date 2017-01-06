using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.FullTextSearch
{
    public class FullTextSearchMatchPart : IBooleanPart
    {
        internal readonly ITsQuery Query;
        internal readonly ITsVector Vector;

        public FullTextSearchMatchPart(ITsVector vector, ITsQuery query)
        {
            Vector = vector;
            Query = query;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}