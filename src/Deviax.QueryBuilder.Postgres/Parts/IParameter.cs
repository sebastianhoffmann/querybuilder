using NpgsqlTypes;

namespace Deviax.QueryBuilder.Parts
{
    public partial interface IParameter<T> : INamedPart, IPart, IParameter {}

    public partial interface IParameter
    {
        NpgsqlDbType? NpgsqlDbType { get; }
    }
}