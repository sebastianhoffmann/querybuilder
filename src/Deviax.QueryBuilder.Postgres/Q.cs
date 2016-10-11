using System;
using System.Collections.Generic;

using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder;

namespace Deviax.QueryBuilder
{
    public static partial class Q
    {
        [Pure]
        public static ArrayAggPart ArrayAgg(IPart over) => new ArrayAggPart(over);

        [Pure]
        public static ToTsVectorPart ToTsVector(string regconfig, IPart over) => new ToTsVectorPart(regconfig, over);

        [Pure]
        public static ToTsQueryPart ToTsQuery(string regconfig, IPart over) => new ToTsQueryPart(regconfig, over);

        [Pure]
        public static IPart Concat(IPart part, params IPart[] parts)
        {
            if (parts.Length == 0)
                return part;

            if(parts.Length == 1)
                return new StringConcatenation(part, parts[0]);

            var result = new StringConcatenation(part, parts[0]);

            for (int i = 1; i < parts.Length; i++)
            {
                result = new StringConcatenation(result, parts[i]);
            }

            return result;
        }
    }
}