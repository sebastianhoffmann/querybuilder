using System.Collections.Generic;
using System.Linq;
using NpgsqlTypes;

namespace Deviax.QueryBuilder.Parts
{
    public partial class ArrayParameter<T> : Part, IArrayParameter<T>, IParameter
    {
        public ArrayParameter(IEnumerable<T> values, string name, NpgsqlDbType npgsqlDbType)
        {
            Name = name;
            Value = values as T[] ?? values.ToArray();
            NpgsqlDbType = npgsqlDbType;
        }

        public NpgsqlDbType? NpgsqlDbType { get; }
    }
}
