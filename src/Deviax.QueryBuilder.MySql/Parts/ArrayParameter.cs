using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Deviax.QueryBuilder.Parts
{
    public partial class ArrayParameter<T> : Part, IArrayParameter<T>, IParameter
    {
        public ArrayParameter(IEnumerable<T> values, string name, MySqlDbType dbType)
        {
            Name = name;
            Value = values as T[] ?? values.ToArray();
            MySqlDbType = dbType;
        }

        public MySqlDbType? MySqlDbType { get; }

        public void ApplyTo(DbCommand cmd)
        {
            foreach (var item in Value)
            {
                if (MySqlDbType.HasValue)
                {
                    new Parameter<T>(item, Name, MySqlDbType.Value).ApplyTo(cmd);
                }
                else
                {
                    new Parameter<T>(item, Name).ApplyTo(cmd);
                }
            }
        }
    }
}