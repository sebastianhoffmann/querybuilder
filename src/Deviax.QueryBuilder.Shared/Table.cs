using System.Collections.Generic;
using System.Security.Principal;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder
{
    public abstract class Table : IFromPart
    {
        public readonly string? TableName;
        public readonly string? TableSchema;
        public readonly string? TableAlias;
        
        private string? _defaultSelect;

        protected Table(string? tableSchema, string? tableName, string? tableAlias = null)
        {
            TableName = tableName;
            TableSchema = tableSchema;
            TableAlias = tableAlias;
        }

        protected internal Field F(string name)
        {
            if (_defaultSelect == null)
            {
                _defaultSelect = $"{TableAlias ?? TableName}.{name} ";
            }
            else
            {
                _defaultSelect += $", {TableAlias ?? TableName}.{name} ";
            }
            
            return new Field(this, name);
        }

        public virtual void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public virtual string? DefaultSelect()
        {
            return _defaultSelect ?? "* ";
        }
    }

    public abstract class Table<T> : Table where T : Table<T>
    {
        public Table(string tableSchema, string tableName, string? tableAlias = null) : base(tableSchema, tableName, tableAlias) {}
    }
}