using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public partial class SumPart : Part
    {
        public SumPart(IPart over, IBooleanPart filterPart)
        {
            Over = over;
            FilterPart = filterPart;
        }

        internal readonly IBooleanPart? FilterPart;

        public SumPart Filter(IBooleanPart filter)
        {
            return new SumPart(Over, filter);
        }
    }
}