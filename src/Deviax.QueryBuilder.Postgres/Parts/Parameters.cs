using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Npgsql;
using NpgsqlTypes;

namespace Deviax.QueryBuilder.Parts
{

    public partial interface IParameter
    {
        NpgsqlDbType? NpgsqlDbType { get; }
       
    }

    public partial interface IParameter<T> : INamedPart, IPart, IParameter
    {
        
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

        public void ApplyTo(DbCommand cmd)
        {
            if (NpgsqlDbType.HasValue)
            {
                ((NpgsqlCommand)cmd).Parameters.AddWithValue(Name, NpgsqlDbType.Value, Value);
            }
            else
            {
                if (Value is DateTime)
                {
                    ((NpgsqlCommand)cmd).Parameters.AddWithValue(Name, NpgsqlTypes.NpgsqlDbType.TimestampTZ, Value);
                }
                else
                {
                    ((NpgsqlCommand)cmd).Parameters.AddWithValue(Name, Value);
                }
            }
        }

    }

    public partial class ArrayParameter<T> : Part, IArrayParameter<T>, IParameter
    {
       public ArrayParameter(IEnumerable<T> values, string name, NpgsqlDbType npgsqlDbType)
        {
            Name = name;
            Value = values as T[] ?? values.ToArray();
            NpgsqlDbType = npgsqlDbType;
        }
        
        public NpgsqlDbType? NpgsqlDbType { get; }

        public void ApplyTo(DbCommand cmd)
        {
            if (NpgsqlDbType.HasValue)
            {
                ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, NpgsqlDbType.Value, Value);
            }
            else
            {

                ((NpgsqlCommand) cmd).Parameters.AddWithValue(Name, Value);

            }
        }
    }
}