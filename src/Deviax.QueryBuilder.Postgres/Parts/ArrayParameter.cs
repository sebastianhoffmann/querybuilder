using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Npgsql;
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

        public void ApplyTo(DbCommand cmd)
        {
            if (NpgsqlDbType.HasValue)
            {
                ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, NpgsqlDbType.Value, Value);
            }
            else
            {
                var dbt = TypeToNpgsqlDbType<T>.NpgsqlDbType;
                if (dbt == null)
                {
                    ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, Value);
                }
                else
                {
                    ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, NpgsqlTypes.NpgsqlDbType.Array | dbt.Value, Value);
                }
            }
        }
    }
}