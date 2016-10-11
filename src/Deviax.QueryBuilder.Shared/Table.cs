using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder
{
    public abstract class Table : IFromPart
    {
        public readonly string TableName;
        public readonly string TableSchema;
        public readonly string TableAlias;

        protected Table(string tableSchema, string tableName, string tableAlias = null)
        {
            TableName = tableName;
            TableSchema = tableSchema;
            TableAlias = tableAlias;
        }

        protected internal Field F(string name) => new Field(this, name);

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public abstract class Table<T> : Table where T : Table<T>
    {
        public Table(string tableSchema, string tableName, string tableAlias = null) : base(tableSchema, tableName, tableAlias) {}
    }
}