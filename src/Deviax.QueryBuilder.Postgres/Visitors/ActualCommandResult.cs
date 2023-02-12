using System;
using System.Collections.Generic;
using System.Linq;
using Deviax.QueryBuilder.Parts;
using NetTopologySuite.Geometries;
using NodaTime;
using Npgsql;
using NpgsqlTypes;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class ActualCommandResult
    {
        private readonly Dictionary<string, NpgsqlParameter> _parameters = new();

        public void AddParameter<T>(IParameter<T> para)
        {
            var val = Equals(null, para.Value) ? (object) DBNull.Value : para.Value;

            NpgsqlParameter p;

            if (_parameters.TryGetValue(para.Name, out p!))
            {
                p.Value = val;
            }
            else
            {
                if (para.NpgsqlDbType.HasValue)
                {
                    _parameters[para.Name] = new NpgsqlParameter(para.Name, para.NpgsqlDbType.Value) { Value = val };
                }
                else
                {
                    if (val is DateTime)
                    {
                        _parameters[para.Name] = new NpgsqlParameter(para.Name, NpgsqlDbType.TimestampTz) { Value = val };
                    }
                    else if (val is Instant or LocalDateTime)
                    {
                        _parameters[para.Name] = new NpgsqlParameter(para.Name, NpgsqlDbType.TimestampTz) { Value = val };
                    }
                    else if (val is ZonedDateTime or OffsetDateTime or DateTimeOffset)
                    {
                        _parameters[para.Name] = new NpgsqlParameter(para.Name, NpgsqlDbType.TimestampTz) { Value = val };
                    }
                    else if (val is Geometry)
                    {
                        _parameters[para.Name] = new NpgsqlParameter(para.Name, NpgsqlDbType.Geometry) { Value = val };
                    }
                    else
                    {
                        _parameters[para.Name] = new NpgsqlParameter(para.Name, val);
                    }
                }
            }
        }

        public void AddParameters()
        {
            Command.Parameters.AddRange(_parameters.Values.ToArray());
        }
    }
}