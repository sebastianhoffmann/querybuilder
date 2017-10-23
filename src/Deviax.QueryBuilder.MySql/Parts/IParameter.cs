using MySql.Data.MySqlClient;

namespace Deviax.QueryBuilder.Parts
{
    public partial interface IParameter<T> : INamedPart, IPart, IParameter {}

    public partial interface IParameter
    {
        MySqlDbType? MySqlDbType { get; }
    }
}