using System;
using System.Collections.Generic;
using System.Data.Common;
using Deviax.QueryBuilder.Parts;
using Npgsql;
using NpgsqlTypes;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class ActualCommandResult
    {
        public void AddParameter<T>(IParameter<T> para)
        {
            var val = Equals(null, para.Value) ? (object)DBNull.Value : para.Value;

            if (Command.Parameters.Contains(para.Name))
            {
                Command.Parameters[para.Name].Value = val;
            }
            else
            {
                if (para.NpgsqlDbType.HasValue)
                {
                    ((NpgsqlCommand) Command).Parameters.AddWithValue(para.Name, para.NpgsqlDbType.Value, val);
                }
                else
                {
                    if (val is DateTime)
                    {
                        ((NpgsqlCommand)Command).Parameters.AddWithValue(para.Name, NpgsqlDbType.TimestampTZ, val);
                    }
                    else
                    {
                        ((NpgsqlCommand)Command).Parameters.AddWithValue(para.Name, val);
                    }
                }
            }
        }
    }
    
}