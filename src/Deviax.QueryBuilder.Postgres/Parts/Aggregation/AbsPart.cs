using System;
using System.Collections.Generic;
using System.Text;

namespace Deviax.QueryBuilder.Parts.Aggregation
{
    public partial class AbsPart : Part
    {
        public AbsPart(IPart over, IBooleanPart filterPart)
        {
            Over = over;
            FilterPart = filterPart;
        }

        internal readonly IBooleanPart FilterPart;

        public AbsPart Filter(IBooleanPart filter)
        {
            return new AbsPart(Over, filter);
        }
    }
}
