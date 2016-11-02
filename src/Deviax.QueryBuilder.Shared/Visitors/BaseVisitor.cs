using System;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public enum CoarseState
    {
        Select,
        FromAndJoins,
        Where,
        GroupBy,
        Having,
        OrderBy,
        ExtraParameters,
        Target,
        Set,
        ValuesDeclaration,
        Values,
        Returning,
    }

    public abstract partial class BaseVisitor : INodeVisitor
    {
        protected readonly IVisitorResult Result;
        protected CoarseState State;
        public BaseVisitor(IVisitorResult result)
        {
            Result = result;
            State = CoarseState.Select;
        }
        protected bool ExpectsFqn;
        protected bool NoTableName;

        public void Visit(PlusPart plus) => HandleOperation(plus.Left, plus.Right, "+ ");
        public void Visit(MinusPart minus) => HandleOperation(minus.Left, minus.Right, "- ");
        public void Visit(MulPart mul) => HandleOperation(mul.Left, mul.Right, "* ");
        public void Visit(DivPart div) => HandleOperation(div.Left, div.Right, "/ ");
        public void Visit(ModPart modPart) => HandleOperation(modPart.Left, modPart.Right, "% ");
        public void Visit(LowerPart lowerPart)
        {
            Result.Append("LOWER(");
            lowerPart.Over.Accept(this);
            Result.Append(")");
        }

        
        public virtual void Visit(CasePart casePart)
        {
            Result.Append("(CASE ");

            foreach (var when in casePart.Whens)
            {
                Result.Append("\n").Append("WHEN ");
                when.Item1.Accept(this);
                Result.Append(" THEN ");
                when.Item2.Accept(this);
            }

            if (casePart.ElsePart != null)
            {
                Result.Append("\n").Append("ELSE ");
                casePart.ElsePart.Accept(this);
            }

            Result.Append(" END) ");
        }

        public void Visit(AndPart and) => HandleOperation(and.Left, and.Right, "AND ");
        public void Visit(OrPart or) => HandleOperation(or.Left, or.Right, "OR ");
        public void Visit(EqPart eq) => HandleOperation(eq.Left, eq.Right, "= ");

        public void Visit<T>(IParameter<T> parameter)
        {
            if (State == CoarseState.ExtraParameters)
            {
                Result.AddParameter(parameter);
            }
            else
            {
                Result.Append("@").Append(parameter.Name).Append(" ").AddParameter(parameter);
            }
        }

        public void Visit(RawSql rawSql) => Result.Append(rawSql.Sql).Append(" ");
        public void Visit(GtePart gte) => HandleOperation(gte.Left, gte.Right, ">= ");
        public void Visit(LtePart lte) => HandleOperation(lte.Left, lte.Right, "<= ");
        public void Visit(GtPart gt) => HandleOperation(gt.Left, gt.Right, "> ");
        public void Visit(LtPart lt) => HandleOperation(lt.Left, lt.Right, "< ");

        public void Visit(IsNotNullPart notNull)
        {
            notNull.Part.Accept(this);
            Result.Append("IS NOT NULL ");
        }
        
        

        public void Visit(IsNullPart isNull)
        {
            isNull.Part.Accept(this);
            Result.Append("IS NULL ");
        }

        public virtual void Visit(NeqPart neq) => HandleOperation(neq.Left, neq.Right, "<> ");

        public void Visit(BetweenPart betweenPart)
        {
            betweenPart.IntFieldPart.Accept(this);
            Result.Append("BETWEEN ");
            betweenPart.Left.Accept(this);
            Result.Append(" AND ");
            betweenPart.Right.Accept(this);
        }

        public virtual void Visit(IsFalsePart part)
        {
            Result.Append("NOT ");
            part.Part.Accept(this);
        }

        public void Visit(MaxPart max)
        {
            Result.Append("MAX(");
            max.Over.Accept(this);
            Result.Append(") ");
        }

        public void Visit(MinPart min)
        {
            Result.Append("MIN(");
            min.Over.Accept(this);
            Result.Append(") ");
        }

        public virtual void Visit<T>(Literal<T> literal)
        {
            var t = typeof (T);

            if (t == typeof (string))
            {
                Result.Append("'").Append(literal.Value as string).Append("' ");
            }
            else
            {
                Result.Append(literal.Value.ToString()).Append(" ");
            }
        }

        public virtual void Visit(IsTruePart truePart) => truePart.Part.Accept(this);

        protected void HandleOperation(IPart left, IPart right, string between)
        {
            Result.Append("(");
            left.Accept(this);
            Result.Append(between);
            right.Accept(this);
            Result.Append(") ");
        }

        public void Visit(BaseUpdateQuery updateQuery)
        {
            Result.Append("(");
            new UpdateVisitor(Result).Process(updateQuery);
            Result.Append(")");
        }

        public void Visit(BaseSelectQuery query)
        {
            Result.Append("(");
            new SelectVisitor(Result).Process(query);
            Result.Append(")");
        }

        public void Visit(BaseDeleteQuery query)
        {
            Result.Append("(");
            new DeleteVisitor(Result).Process(query);
            Result.Append(")");
        }

        protected void TransitionToExtraParameters()
        {
            State = CoarseState.ExtraParameters;
        }

        protected virtual void TransitionToFromPart()
        {
            State = CoarseState.FromAndJoins;
        }

        protected virtual void TransitionToTargetPart()
        {
            State = CoarseState.Target;
        }

        protected virtual void TransitionToSetPart()
        {
            State = CoarseState.Set;
        }

        protected virtual void TransitionToJoins() {}

        protected virtual void TransitionToWhere()
        {
            State = CoarseState.Where;
        }

        protected virtual void TransitionToGroupBy()
        {
            State = CoarseState.GroupBy;
        }

        protected virtual void TransitionToHaving()
        {
            State = CoarseState.Having;
        }

        protected virtual void TransitionToOrderBy()
        {
            State = CoarseState.OrderBy;
        }

        protected virtual void TransitionToValuesDeclaration()
        {
            State = CoarseState.ValuesDeclaration;
        }

        protected virtual void TransitionToValues()
        {
            State = CoarseState.Values;
        }

        protected virtual void TransitionToReturning()
        {
            State = CoarseState.Returning;
        }

        public abstract void Visit(SetFieldPart sfp);
        
        public abstract void Visit(DistinctPart distinctPart);
        public abstract void Visit(InnerJoinPart innerJoinPart);
        public abstract void Visit(DescOrdering desc);
        public abstract void Visit(AscOrdering asc);
        public abstract void Visit(RightJoinPart rightJoinPart);
        public abstract void Visit(LeftJoinPart leftJoinPart);
        public abstract void Visit(SumPart sumPart);
        public abstract void Visit(CountPart countPart);
        public virtual void Visit(CoalescePart coalesce)
        {
            Result.Append("COALESCE(");
            coalesce.Over[0].Accept(this);
            for (int i = 1; i < coalesce.Over.Length; i++)
            {
                Result.Append(", ");
                coalesce.Over[i].Accept(this);
            }
            Result.Append(")");
        }
        public abstract void Visit(LimitOffsetPart limitOffset);
    }
}