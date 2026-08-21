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
    }
}
