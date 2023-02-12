using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public partial class MaxPart : Part
    {
        public MaxPart(IPart over, IBooleanPart filterPart)
        {
            Over = over;
            FilterPart = filterPart;
        }

        internal readonly IBooleanPart? FilterPart;

        public MaxPart Filter(IBooleanPart filter)
        {
            return new MaxPart(Over, filter);
        }
    }
}