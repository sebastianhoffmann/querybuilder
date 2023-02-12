namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public partial class CountPart : Part
    {
        public CountPart(IPart over, IBooleanPart filterPart)
        {
            Over = over;
            FilterPart = filterPart;
        }

        internal readonly IBooleanPart? FilterPart;

        public CountPart Filter(IBooleanPart filter)
        {
            return new CountPart(Over, filter);
        }
    }
}