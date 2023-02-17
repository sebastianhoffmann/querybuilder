using System;
using System.Collections.Generic;
using System.Linq;
using Deviax.QueryBuilder.Parts;
using Npgsql;
using NpgsqlTypes;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class ActualCommandResult
    {
        private readonly Dictionary<string, NpgsqlParameter> _parameters = new();

        public void AddParameter<T>(IParameter<T> para)
        {
            var val = para.Value is null ? (object)DBNull.Value : para.Value;

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
                    var dbt = TypeToNpgsqlDbType<T>.NpgsqlDbType;
                    if (dbt == null)
                    {
                        _parameters[para.Name] = new NpgsqlParameter(para.Name, val);
                    }
                    else
                    {
                        _parameters[para.Name] = new NpgsqlParameter(para.Name, dbt.Value) { Value = val };
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