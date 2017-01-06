using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.FullTextSearch
{
    public class ToTsQueryPart : Part, ITsQuery
    {
        internal readonly IPart Over;
        internal readonly string Regconfig;

        public ToTsQueryPart(string regconfig, IPart over)
        {
            Regconfig = regconfig;
            Over = over;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}