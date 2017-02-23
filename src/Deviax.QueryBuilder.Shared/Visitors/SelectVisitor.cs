using System;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Parts.Aggregation;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class SelectVisitor : BaseVisitor
    {
        public SelectVisitor(IVisitorResult result) : base(result)
        {
            State = CoarseState.Select;
        }
        
        public override void Visit(RightJoinPart rightJoinPart)
        {
            Result.Append("\nRIGHT JOIN ");
            ExpectsFqn = true;
            rightJoinPart.From.Accept(this);
            ExpectsFqn = false;
            Result.Append(" ON ");

            rightJoinPart.Conditions[0].Accept(this);
            for (var i = 1; i < rightJoinPart.Conditions.Length; i++)
            {
                Result.Append("AND ");
                rightJoinPart.Conditions[i].Accept(this);
            }
        }

        public override void Visit(LeftJoinPart leftJoinPart)
        {
            Result.Append("\nLEFT JOIN ");
            ExpectsFqn = true;
            leftJoinPart.From.Accept(this);
            ExpectsFqn = false;
            Result.Append(" ON ");

            leftJoinPart.Conditions[0].Accept(this);
            for (var i = 1; i < leftJoinPart.Conditions.Length; i++)
            {
                Result.Append("AND ");
                leftJoinPart.Conditions[i].Accept(this);
            }
        }
        
        public override void Visit(DistinctPart distinctPart)
        {
            Result.Append("DISTINCT(");
            distinctPart.Over[0].Accept(this);
            for (int i = 1; i < distinctPart.Over.Length; i++)
            {
                Result.Append(", ");
                distinctPart.Over[i].Accept(this);
            }
            Result.Append(")");
        }
        
        public override void Visit(InnerJoinPart innerJoinPart)
        {
            Result.Append("\nINNER JOIN ");
            ExpectsFqn = true;
            innerJoinPart.From.Accept(this);
            ExpectsFqn = false;
            Result.Append(" ON ");

            innerJoinPart.Conditions[0].Accept(this);
            for (var i = 1; i < innerJoinPart.Conditions.Length; i++)
            {
                Result.Append("AND ");
                innerJoinPart.Conditions[i].Accept(this);
            }
        }
        
        public override void Visit(DescOrdering desc)
        {
            desc.Part.Accept(this);
            Result.Append("DESC ");

            switch (desc.Nulls)
            {
                case Nulls.First:
                    Result.Append("NULLS FIRST ");
                    break;
                case Nulls.Last:
                    Result.Append("NULLS LAST ");
                    break;
                case Nulls.Unspecified:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Visit(AscOrdering asc)
        {
            asc.Part.Accept(this);
            Result.Append("ASC ");

            switch (asc.Nulls)
            {
                case Nulls.First:
                    Result.Append("NULLS FIRST ");
                    break;
                case Nulls.Last:
                    Result.Append("NULLS LAST ");
                    break;
                case Nulls.Unspecified:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override void Visit(SetFieldPart sfp)
        {
            throw new NotSupportedException();
        }

        public virtual void Process(BaseSelectQuery q)
        {
            if (q.SelectParts == null || q.SelectParts.Count == 0)
            {
                if (LimitWithoutOffsetAtStart && q.LimitOffsetPart != null && q.LimitOffsetPart.Offset == null)
                {
                    Result.Append("SELECT ");
                    q.LimitOffsetPart.Accept(this);
                    Result.Append(" *");
                }
                else
                {
                    Result.Append("SELECT *");
                }
            }
            else
            {
                Result.Append("SELECT ");

                if (LimitWithoutOffsetAtStart && q.LimitOffsetPart != null && q.LimitOffsetPart.Offset == null)
                {
                    q.LimitOffsetPart.Accept(this);
                    Result.Append(" ");
                }

                q.SelectParts[0].Accept(this);

                for (int i = 1; i < q.SelectParts.Count; i++)
                {
                    Result.Append(", ");
                    q.SelectParts[i].Accept(this);
                }
            }

            TransitionToFromPart();

            if (q.From != null)
            {
                Result.Append("\nFROM ");
                ExpectsFqn = true;
                q.From.Accept(this);
                ExpectsFqn = false;
            }

            TransitionToJoins();

            if (q.Joins != null && q.Joins.Count > 0)
            {
                foreach (var p in q.Joins)
                    p.Accept(this);
            }

            TransitionToWhere();

            if (q.WhereParts != null && q.WhereParts.Count > 0)
            {
                Result.Append("\nWHERE ");

                q.WhereParts[0].Accept(this);

                for (int i = 1; i < q.WhereParts.Count; i++)
                {
                    Result.Append("AND ");
                    q.WhereParts[i].Accept(this);
                }
            }

            TransitionToGroupBy();

            if (q.GroupByParts != null && q.GroupByParts.Count > 0)
            {
                Result.Append("\nGROUP BY ");
                q.GroupByParts[0].Accept(this);

                for (int i = 1; i < q.GroupByParts.Count; i++)
                {
                    Result.Append(", ");
                    q.GroupByParts[i].Accept(this);
                }
            }

            TransitionToHaving();

            if (q.HavingParts != null && q.HavingParts.Count > 0)
            {
                Result.Append("\nHAVING ");

                q.HavingParts[0].Accept(this);

                for (int i = 1; i < q.HavingParts.Count; i++)
                {
                    Result.Append("AND ");
                    q.HavingParts[i].Accept(this);
                }
            }

            TransitionToOrderBy();

            if (q.OrderByParts != null && q.OrderByParts.Count > 0)
            {
                Result.Append("\nORDER BY ");

                q.OrderByParts[0].Accept(this);

                for (int i = 1; i < q.OrderByParts.Count; i++)
                {
                    Result.Append(", ");
                    q.OrderByParts[i].Accept(this);
                }
            }
            
            if (!LimitWithoutOffsetAtStart || q.LimitOffsetPart?.Offset != null)
                q.LimitOffsetPart?.Accept(this);

            TransitionToExtraParameters();

            if (q.ExtraParameters != null)
            {
                foreach (var para in q.ExtraParameters)
                    para.Accept(this);
            }
        }
    }
}