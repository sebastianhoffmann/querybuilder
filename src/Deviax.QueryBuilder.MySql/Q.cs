using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Parts;
using MySql.Data.MySqlClient;

namespace Deviax.QueryBuilder
{
    public static partial class Q
    {
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
        public static Parameter<T> P<T>(MySqlDbType dbType, string name, T value) => new Parameter<T>(value, name, dbType);

        [Pure]
        public static Parameter<T> Parameter<T>(MySqlDbType dbType, string name, T value) => P(dbType, name, value);

        [Pure]
        public static ArrayParameter<T> AP<T>(MySqlDbType dbType, string name, params T[] values) => new ArrayParameter<T>(values, name, dbType);

        [Pure]
        public static ArrayParameter<T> ArrayParameter<T>(MySqlDbType dbType, string name, params T[] values) => AP(dbType, name, values);
    }
}