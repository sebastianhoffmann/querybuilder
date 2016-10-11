using Deviax.QueryBuilder.Parts;
using System;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class UpdateVisitor : BaseVisitor
    {
        public UpdateVisitor(IVisitorResult result) : base(result)
        {
        }

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

            
            if (q.From != null)
            {
                TransitionToFromPart();
                Result.Append("\nFROM ");
                ExpectsFqn = true;
                q.From.Accept(this);
                ExpectsFqn = false;
            }

            if (q.WhereParts != null && q.WhereParts.Count > 0)
            {
                TransitionToWhere();
                Result.Append("\nWHERE ");
                q.WhereParts[0].Accept(this);

                for (int i = 1; i < q.WhereParts.Count; i++)
                {
                    Result.Append(", ");
                    q.WhereParts[i].Accept(this);
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

        public override void Visit(CoalescePart coalesce)
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