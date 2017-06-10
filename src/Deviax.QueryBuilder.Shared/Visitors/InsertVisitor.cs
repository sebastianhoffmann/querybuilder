using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Parts.Aggregation;
using System;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class InsertVisitor : BaseVisitor, IQueryingVisitor<BaseInsertQuery>
    {
        public void Process(BaseInsertQuery q)
        {

            Result.Append("INSERT INTO ");
            TransitionToTargetPart();
            ExpectsFqn = true;
            q.Target.Accept(this);
            ExpectsFqn = false;
            TransitionToValuesDeclaration();
            Result.Append("\n( ");

            var firstValueCollection = q.Values[0];
            firstValueCollection.Values[0].Accept(this);
            
            for (int i = 1; i < firstValueCollection.Values.Count; i++)
            {
                Result.Append(", ");
                firstValueCollection.Values[i].Accept(this);
            }

            Result.Append(") VALUES \n(");

            TransitionToValues();

            firstValueCollection.Values[0].Accept(this);
            for (int i = 1; i < firstValueCollection.Values.Count; i++)
            {
                Result.Append(", ");
                firstValueCollection.Values[i].Accept(this);
            }

            for (int i = 1; i < q.Values.Count; i++)
            {
                Result.Append("),\n(");

                q.Values[i].Values[0].Accept(this);
                for (int j = 1; j < q.Values[i].Values.Count; j++)
                {
                    Result.Append(", ");
                    q.Values[i].Values[j].Accept(this);
                }
            }
            Result.Append(") ");


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