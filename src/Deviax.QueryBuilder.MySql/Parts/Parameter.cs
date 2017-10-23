using System;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Deviax.QueryBuilder.Parts
{
    public partial class Parameter<T> : Part, IParameter<T>
    {
        public Parameter(T value, string name, MySqlDbType dbType)
        {
            Name = name;
            Value = value;
            MySqlDbType = dbType;
        }

        public MySqlDbType? MySqlDbType { get; }

        public void ApplyTo(DbCommand cmd)
        {
            if (MySqlDbType.HasValue)
            {
                ((MySqlCommand) cmd).Parameters.Add(new MySqlParameter(Name, MySqlDbType.Value) {Value =  Value });
            }
            else
            {
                if (Value is DateTime)
                {
                    ((MySqlCommand) cmd).Parameters.Add(new MySqlParameter(Name, MySql.Data.MySqlClient.MySqlDbType.DateTime) {Value =  Value });
                }
                else
                {
                    ((MySqlCommand) cmd).Parameters.AddWithValue(Name, Value);
                }
            }
        }
    }
}