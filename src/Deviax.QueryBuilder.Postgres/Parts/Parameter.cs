using System;
using System.Data.Common;
using NetTopologySuite.Geometries;
using NodaTime;
using Npgsql;
using NpgsqlTypes;

namespace Deviax.QueryBuilder.Parts
{
    public partial class Parameter<T> : Part, IParameter<T>
    {
        public Parameter(T value, string name, NpgsqlDbType npgsqlDbType)
        {
            Name = name;
            Value = value;
            NpgsqlDbType = npgsqlDbType;
        }

        public NpgsqlDbType? NpgsqlDbType { get; }

        public void ApplyTo(DbCommand cmd)
        {
            if (NpgsqlDbType.HasValue)
            {
                ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, NpgsqlDbType.Value, Value!);
            }
            else
            {
                if (Value is DateTime)
                {
                    ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, NpgsqlTypes.NpgsqlDbType.TimestampTz, Value);
                }
                else if (Value is Instant or LocalDateTime)
                {
                    ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, NpgsqlTypes.NpgsqlDbType.TimestampTz, Value);
                }
                else if (Value is ZonedDateTime or OffsetDateTime or DateTimeOffset)
                {
                    ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, NpgsqlTypes.NpgsqlDbType.TimestampTz, Value);
                }
                else if (Value is Geometry)
                {
                    ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, NpgsqlTypes.NpgsqlDbType.Geometry, Value);
                }
                else
                {
                    ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, Value!);
                }
            }
        }
    }
}