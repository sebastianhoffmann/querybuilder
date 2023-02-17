using NpgsqlTypes;

namespace Deviax.QueryBuilder;

public static class TypeToNpgsqlDbType<T>
{
    public static NpgsqlDbType? NpgsqlDbType;
}