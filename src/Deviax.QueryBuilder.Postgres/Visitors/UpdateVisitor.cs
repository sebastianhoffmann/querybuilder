using System;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class UpdateVisitor
    {
        public override void Visit(SetFieldPart sfp)
        {
            NoTableName = true;
            sfp.Field.Accept(this);
            NoTableName = false;
            Result.Append("= ");
            sfp.Value.Accept(this);
        }

        public override void Visit(RowNumberPart rowNumberPart)
        {
            throw new NotSupportedException();
        }

        public override void Visit(PartitionPart partitionPart)
        {
            throw new NotSupportedException();
        }
    }
}