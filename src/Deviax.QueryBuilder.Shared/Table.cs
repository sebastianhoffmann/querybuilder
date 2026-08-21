using System;
using System.Collections.Generic;
using System.Security.Principal;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder
{
    public abstract partial class Table : IFromPart
    {
        public readonly string? TableName;
        public readonly string? TableSchema;
        public readonly string? TableAlias;

        internal readonly Dictionary<string, Field> Fields = new(StringComparer.Ordinal);
        
        private string? _defaultSelect;

        protected Table(string? tableSchema, string? tableName, string? tableAlias = null)
        {
            TableName = tableName;
            TableSchema = tableSchema;
            TableAlias = tableAlias;
        }

        protected internal Field F(string name)
        {
            var field = new Field(this, name);
            Fields.Add(name, field);

            if (_defaultSelect == null)
            {
                _defaultSelect = $"{TableAlias ?? TableName}.{name} ";
            }
            else
            {
                _defaultSelect += $", {TableAlias ?? TableName}.{name} ";
            }
            
            return field;
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
