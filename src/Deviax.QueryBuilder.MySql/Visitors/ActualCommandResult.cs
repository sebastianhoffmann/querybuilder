using System;
using System.Collections.Generic;
using System.Linq;
using Deviax.QueryBuilder.Parts;
using MySql.Data.MySqlClient;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class ActualCommandResult
    {
        private readonly Dictionary<string, MySqlParameter> _parameters = new Dictionary<string, MySqlParameter>();

        public void AddParameter<T>(IParameter<T> para)
        {
            var val = Equals(null, para.Value) ? (object) DBNull.Value : para.Value;

            MySqlParameter? p;

            if (_parameters.TryGetValue(para.Name, out p))
            {
                p.Value = val;
            }
            else
            {
                if (para.MySqlDbType.HasValue)
                {
                    _parameters[para.Name] = new MySqlParameter(para.Name, para.MySqlDbType.Value) {Value = val};
                }
                else
                {
                    if (val is DateTime)
                    {
                        _parameters[para.Name] = new MySqlParameter(para.Name, MySqlDbType.DateTime) {Value = val};
                    }
                    else
                    {
                        _parameters[para.Name] = new MySqlParameter(para.Name, val);
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