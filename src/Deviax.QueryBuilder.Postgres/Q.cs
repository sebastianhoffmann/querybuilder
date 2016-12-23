using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Parts;
using NpgsqlTypes;

namespace Deviax.QueryBuilder
{
    public enum Extractable
    {
        Year,
        Month,
        Day
    }

    public static partial class Q
    {
        [Pure]
        public static RowNumberPart RowNumber(IPartitionOverPart over) => new RowNumberPart(over);

        [Pure]
        public static PartitionPart PartitionBy(IPart thing) => new PartitionPart(thing);

        [Pure]
        public static ArrayAggPart ArrayAgg(IPart over) => new ArrayAggPart(over);

        [Pure]
        public static ToTsVectorPart ToTsVector(string regconfig, IPart over) => new ToTsVectorPart(regconfig, over);

        [Pure]
        public static ToTsQueryPart ToTsQuery(string regconfig, IPart over) => new ToTsQueryPart(regconfig, over);

        [Pure]
        public static CastPart Cast(IPart what, string targetType) => new CastPart(what, targetType);

        [Pure]
        public static AgePart Age(IPart what) => new AgePart(what);

        [Pure]
        public static ExtractPart Extract(Extractable what, IPart from) => new ExtractPart(what, from);

        [Pure]
        public static IPart Concat(IPart part, params IPart[] parts)
        {
            if (parts.Length == 0)
                return part;

            if (parts.Length == 1)
                return new StringConcatenation(part, parts[0]);

            var result = new StringConcatenation(part, parts[0]);

            for (var i = 1; i < parts.Length; i++)
            {
                result = new StringConcatenation(result, parts[i]);
            }

            return result;
        }

        [Pure]
        public static Parameter<T> P<T>(NpgsqlDbType npgsqlType, string name, T value) => new Parameter<T>(value, name, npgsqlType);

        [Pure]
        public static Parameter<T> Parameter<T>(NpgsqlDbType npgsqlType, string name, T value) => P(npgsqlType, name, value);

        [Pure]
        public static ArrayParameter<T> AP<T>(NpgsqlDbType npgsqlType, string name, params T[] values) => new ArrayParameter<T>(values, name, npgsqlType);

        [Pure]
        public static ArrayParameter<T> ArrayParameter<T>(NpgsqlDbType npgsqlType, string name, params T[] values) => AP(npgsqlType, name, values);
    }
}