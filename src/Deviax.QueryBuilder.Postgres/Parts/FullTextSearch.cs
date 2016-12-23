using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public static class Regconfig
    {
        public const string German = "german";
        public const string English = "English";
    }
    
    public interface ITsVector : IPart { }
    public interface ITsQuery : IPart { }

    public class MatchesRegexPart : Part, IBooleanPart
    {
        public readonly IPart Left;
        public readonly IPart Right;

        public MatchesRegexPart(IPart left, IPart right)
        {
            Left = left;
            Right = right;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Accept(this);
        }
    }

    public class ToTsVectorPart : Part, ITsVector
    {
        internal readonly string Regconfig;
        internal readonly IPart Over;
        
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

    public class FullTextSearchMatchPart : IBooleanPart
    {
        internal readonly ITsVector Vector;
        internal readonly ITsQuery Query;

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

    public class ToTsQueryPart : Part, ITsQuery
    {
        internal readonly string Regconfig;
        internal readonly IPart Over;

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

    public class FullTextSearch
    {

        //where to_tsvector('german', p.name || long_description || short_description) @@ to_tsquery('german', 'nudel')


    }
}