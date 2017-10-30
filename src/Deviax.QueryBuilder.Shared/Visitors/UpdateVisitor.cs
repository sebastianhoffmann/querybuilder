using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Parts.Aggregation;
using System;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class UpdateVisitor : BaseVisitor, IQueryingVisitor<BaseUpdateQuery>
    {
        public void Process(BaseUpdateQuery q)
        {
            Result.Append("UPDATE ");
            TransitionToTargetPart();
            ExpectsFqn = true;
            q.Target.Accept(this);
            ExpectsFqn = false;
            TransitionToSetPart();
            Result.Append("\nSET ");

            q.SetParts[0].Accept(this);

            for (int i = 1; i < q.SetParts.Count; i++)
            {
                Result.Append(", ");
                q.SetParts[i].Accept(this);
            }
            
            if (q.FromPart != null)
            {
                TransitionToFromPart();
                Result.Append("\nFROM ");
                ExpectsFqn = true;
                q.FromPart.Accept(this);
                ExpectsFqn = false;
            }

            if (q.WhereParts != null && q.WhereParts.Count > 0)
            {
                TransitionToWhere();
                Result.Append("\nWHERE ");
                q.WhereParts[0].Accept(this);

                for (int i = 1; i < q.WhereParts.Count; i++)
                {
                    Result.Append("AND ");
                    q.WhereParts[i].Accept(this);
                }
            }

            if (q.ReturningParts != null && q.ReturningParts.Count > 0)
            {
                TransitionToReturning();
                Result.Append("\n RETURNING ");
                q.ReturningParts[0].Accept(this);

                for (int i = 1; i < q.ReturningParts.Count; i++)
                {
                    Result.Append(", ");
                    q.ReturningParts[i].Accept(this);
                }
            }

            TransitionToExtraParameters();

            if (q.ExtraParameters != null)
            {
                foreach (var para in q.ExtraParameters)
                    para.Accept(this);
            }
        }

        public override void Visit(RightJoinPart rightJoinPart)
        {
            throw new NotSupportedException();
        }

        public override void Visit(LeftJoinPart leftJoinPart)
        {
            throw new NotSupportedException();
        }

        public override void Visit(SumPart sumPart)
        {
            throw new NotSupportedException();
        }

        public override void Visit(CountPart countPart)
        {
            throw new NotSupportedException();
        }

        public override void Visit(DistinctPart distinctPart)
        {
            throw new NotSupportedException();
        }

        public override void Visit(InnerJoinPart innerJoinPart)
        {
            throw new NotSupportedException();
        }

        public override void Visit(DescOrdering desc)
        {
            throw new NotSupportedException();
        }

        public override void Visit(AscOrdering asc)
        {
            throw new NotSupportedException();
        }

        public override void Visit(LimitOffsetPart limitOffset)
        {
            throw new NotSupportedException();
        }
    }
}