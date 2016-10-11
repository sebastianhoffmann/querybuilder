using System.Collections.Generic;
using System.Linq;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public interface IArrayParameter<T> : IParameter<T[]> { }

    public interface IParameter<T> : INamedPart, IPart
    {
        T Value { get; }
    }

    public class Parameter<T> : Part, IParameter<T>
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

    public class ArrayParameter<T> : Part, IArrayParameter<T>
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