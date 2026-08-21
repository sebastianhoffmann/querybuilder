using NpgsqlTypes;

namespace Deviax.QueryBuilder
{
    public abstract partial class Table
    {
        protected internal Field F(string name, NpgsqlDbType npgsqlDbType)
        {
            var field = F(name);
            field.NpgsqlDbType = npgsqlDbType;
            return field;
        }
    }
}
