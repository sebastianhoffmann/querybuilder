using System;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class DeleteVisitor
    {
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