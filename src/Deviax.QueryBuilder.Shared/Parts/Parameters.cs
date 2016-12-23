using System.Collections.Generic;
using System.Linq;
using Deviax.QueryBuilder.Visitors;
using System.Data.Common;

namespace Deviax.QueryBuilder.Parts
{
    public interface IArrayParameter<T> : IParameter<T[]> { }

    public partial interface IParameter : INamedPart, IPart
    {
        void ApplyTo(DbCommand cmd);
    }

    public partial interface IParameter<T> : IParameter
    {
        T Value { get; }
    }

    public partial class Parameter<T> : Part, IParameter<T>
    {
        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Parameter(T value, string name)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public T Value { get; }
    }

    public partial class ArrayParameter<T> : Part, IArrayParameter<T>
    {
        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public ArrayParameter(IEnumerable<T> values, string name)
        {
            Name = name;
            Value = values as T[] ?? values.ToArray();
        }

        public string Name { get; }
        public T[] Value { get; }
    }
}