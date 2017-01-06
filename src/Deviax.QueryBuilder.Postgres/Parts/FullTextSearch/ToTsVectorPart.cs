using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.FullTextSearch
{
    public class ToTsVectorPart : Part, ITsVector
    {
        internal readonly IPart Over;
        internal readonly string Regconfig;

        public ToTsVectorPart(string regconfig, IPart over)
        {
            Regconfig = regconfig;
            Over = over;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        [Pure]
        public FullTextSearchMatchPart Match(ITsQuery query) => new FullTextSearchMatchPart(this, query);
    }
}