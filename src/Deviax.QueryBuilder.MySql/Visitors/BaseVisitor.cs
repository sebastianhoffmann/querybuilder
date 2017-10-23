using System;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Parts.Aggregation;

namespace Deviax.QueryBuilder.Visitors
{
    public abstract partial class BaseVisitor
    {
        protected bool LimitWithoutOffsetAtStart => false;

        public void Visit(StringConcatenation sc) => HandleOperation(sc.Left, sc.Right, "|| ");

        public void Visit(LikePart lp) => HandleOperation(lp.Left, lp.Right, "LIKE ");

        public void Visit(IField field)
        {
            if (NoTableName)
            {
                Result.Append("`").Append(field.Name).Append("` ");
            }
            else
            {
                field.Table.Accept(this);
                Result.Append(".`").Append(field.Name).Append("` ");
            }
        }

        public void Visit(IAliasPart aliased)
        {
            aliased.Aliased.Accept(this);

            if (State == CoarseState.Select)
            {
                Result.Append(" AS ").Append(aliased.Name);
            }
        }

        public void Visit(AliasedSelectQuery aliased)
        {
            aliased.Aliased.Accept(this);

            if (State == CoarseState.Select)
            {
                Result.Append(" AS ").Append(aliased.Name);
            }
        }

        public void Visit(Table table)
        {
            if (ExpectsFqn)
            {
                AppendFullyQualifiedTableName(table);
                ExpectsFqn = false;

                if ((State == CoarseState.FromAndJoins || State == CoarseState.Target) && !string.IsNullOrWhiteSpace(table.TableAlias))
                {
                    Result.Append(" AS ").Append(table.TableAlias);
                }
            }
            else if (string.IsNullOrWhiteSpace(table.TableAlias))
            {
                AppendFullyQualifiedTableName(table);
            }
            else
            {
                Result.Append(table.TableAlias);
            }
        }

        public void Visit(MaxPart max)
        {
            Result.Append("MAX(");
            max.Over.Accept(this);
            Result.Append(") ");
        }

        public void Visit(AbsPart absPart)
        {
            Result.Append("ABS(");
            absPart.Over.Accept(this);
            Result.Append(") ");
        }

        public void Visit(MinPart min)
        {
            Result.Append("MIN(");
            min.Over.Accept(this);
            Result.Append(") ");
        }
        
        public void Visit(InPart inPart)
        {
            inPart.Left.Accept(this);
            Result.Append(" IN (");
            inPart.Right.Accept(this);
            Result.Append(")");
        }

        public void Visit<T>(IArrayParameter<T> arrayParam)
        {
            if (State == CoarseState.ExtraParameters)
            {
                Result.AddParameter(arrayParam);
            }
            else
            {
                Result.Append("@").Append(arrayParam.Name).Append("0 ").AddParameter(Q.P(arrayParam.Name + "0", arrayParam.Value[0]));
                for (var i = 1; i < arrayParam.Value.Length; i++)
                {
                    Result.Append(", @").Append(arrayParam.Name).Append(i).Append(" ").AddParameter(Q.P(arrayParam.Name + i, arrayParam.Value[i]));
                }
            }
        }

        public void Visit(IntervalPart interval)
        {
            throw new NotImplementedException();
        }

        public void Visit(CastPart castPart)
        {
            castPart.What.Accept(this);
            Result.Append("::").Append(castPart.TargetType).Append(" ");
        }

        private void AppendFullyQualifiedTableName(Table table)
        {
            if (!string.IsNullOrWhiteSpace(table.TableSchema))
            {
                Result.Append("`").Append(table.TableSchema).Append("`.");
            }

            Result.Append("`").Append(table.TableName).Append("`");
        }
    }
}