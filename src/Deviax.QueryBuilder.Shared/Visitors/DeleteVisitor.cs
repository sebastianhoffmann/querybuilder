using Deviax.QueryBuilder.Parts;
using System;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class DeleteVisitor : BaseVisitor
    {
        public DeleteVisitor(IVisitorResult result) : base(result)
        {
        }
        
        public void Process(BaseDeleteQuery q)
        {
            Result.Append("DELETE FROM ");
            TransitionToTargetPart();
            ExpectsFqn = true;
            q.From.Accept(this);
            ExpectsFqn = false;

            if (q.WhereParts != null && q.WhereParts.Count > 0)
            {
                TransitionToWhere();
                NoTableName = true;
                Result.Append("\nWHERE ");
                q.WhereParts[0].Accept(this);

                for (int i = 1; i < q.WhereParts.Count; i++)
                {
                    Result.Append(", ");
                    q.WhereParts[i].Accept(this);
                }
                NoTableName = false;
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

        public override void Visit(SetFieldPart sfp)
        {
            throw new NotSupportedException();
        }
    }
}