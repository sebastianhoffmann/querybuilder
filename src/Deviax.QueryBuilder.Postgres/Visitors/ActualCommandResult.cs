using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Deviax.QueryBuilder.Parts;
using Npgsql;
using NpgsqlTypes;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class ActualCommandResult
    {
        private readonly Dictionary<string, NpgsqlParameter> _parameters = new Dictionary<string, NpgsqlParameter>();

        public void AddParameters()
        {
            Command.Parameters.AddRange(_parameters.Values.ToArray());
        }

        public void AddParameter<T>(IParameter<T> para)
        {
            var val = Equals(null, para.Value) ? (object)DBNull.Value : para.Value;

            NpgsqlParameter p;

            if (_parameters.TryGetValue(para.Name, out p))
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
                        _parameters[para.Name] = new NpgsqlParameter(para.Name, NpgsqlDbType.TimestampTZ) { Value = val };
                    }
                    else
                    {
                        _parameters[para.Name] = new NpgsqlParameter(para.Name, val);
                    }
                }
            }
        }
    }
    
}