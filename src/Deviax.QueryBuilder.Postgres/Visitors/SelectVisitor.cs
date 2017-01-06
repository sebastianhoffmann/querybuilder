using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class SelectVisitor
    {
        public override void Visit(LimitOffsetPart limitOffset)
        {
            if (limitOffset.Limit.HasValue)
            {
                Result.Append("\nLIMIT ").Append(limitOffset.Limit.Value);
            }

            if (limitOffset.Offset.HasValue)
            {
                Result.Append("\nOFFSET ").Append(limitOffset.Offset.Value);
            }
        }

        public override void Visit(RowNumberPart rowNumberPart)
        {
            Result.Append("row_number() OVER ( ");
            rowNumberPart.Over.Accept(this);
            Result.Append(")");
        }

        public override void Visit(PartitionPart partitionPart)
        {
            Result.Append("\nPARTITION BY ");
            partitionPart.Thing.Accept(this);

            if (partitionPart.OrderPart != null)
            {
                Result.Append("\nORDER BY ");
                partitionPart.OrderPart.Accept(this);
            }
        }
    }
}