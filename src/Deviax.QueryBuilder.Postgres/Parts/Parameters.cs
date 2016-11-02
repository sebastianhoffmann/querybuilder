using System.Collections.Generic;
using System.Linq;
using Deviax.QueryBuilder.Visitors;
using NpgsqlTypes;

namespace Deviax.QueryBuilder.Parts
{

    public partial interface IParameter<T> : INamedPart, IPart
    {
        NpgsqlDbType? NpgsqlDbType { get; }
    }

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

    public partial class ArrayParameter<T> : Part, IArrayParameter<T>
    {
       public ArrayParameter(IEnumerable<T> values, string name, NpgsqlDbType npgsqlDbType)
        {
            Name = name;
            Value = values as T[] ?? values.ToArray();
            NpgsqlDbType = npgsqlDbType;
        }
        
        public NpgsqlDbType? NpgsqlDbType { get; }
    }
}