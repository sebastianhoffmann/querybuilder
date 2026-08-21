using System.Collections.Generic;
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
    }
}
