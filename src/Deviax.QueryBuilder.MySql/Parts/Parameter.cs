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
    }
}
