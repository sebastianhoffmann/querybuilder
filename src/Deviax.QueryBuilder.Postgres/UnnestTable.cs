using System.Collections.Generic;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder
{
    public class UnnestTable : Table
    {
        public UnnestTable(string alias) : base(null, null, alias) { }

        internal List<IParameter> Parameters = new List<IParameter>();

        public Field Field<T>(IEnumerable<T> i)
        {
            var name = $"unest_{Parameters.Count}";
            Parameters.Add(new ArrayParameter<T>(i, name));
            return new Field(this,name);
        }
        
        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}