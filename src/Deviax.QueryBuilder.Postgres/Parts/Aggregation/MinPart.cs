using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public partial class MinPart : Part
    {
        public MinPart(IPart over, IBooleanPart filterPart)
        {
            Over = over;
            FilterPart = filterPart;
        }

        internal readonly IBooleanPart? FilterPart;

        public MinPart Filter(IBooleanPart filter)
        {
            return new MinPart(Over, filter);
        }
    }
}