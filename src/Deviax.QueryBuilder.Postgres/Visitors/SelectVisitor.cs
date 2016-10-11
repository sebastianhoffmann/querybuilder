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
    }
}