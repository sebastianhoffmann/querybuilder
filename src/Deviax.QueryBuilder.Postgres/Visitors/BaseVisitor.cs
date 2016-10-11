using System;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    
    public abstract partial class BaseVisitor 
    {

        public void Visit(StringConcatenation sc) => HandleOperation(sc.Left, sc.Right, "|| ");

        public void Visit(LikePart lp) => HandleOperation(lp.Left, lp.Right, lp.Mode == LikeMode.CaseSensitive ? "LIKE " : "ILIKE ");

        public void Visit(IField field)
        {
            field.Table.Accept(this);
            Result.Append(".\"").Append(field.Name).Append("\" ");
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
            //TODO: probably incorrect?
            aliased.Aliased.Accept(this);

            if (State == CoarseState.Select)
            {
                Result.Append(" AS ").Append(aliased.Name);
            }
        }

        protected bool LimitWithoutOffsetAtStart => false;

        private void AppendFullyQuallifiedTableName(Table table)
        {
            if (!string.IsNullOrWhiteSpace(table.TableSchema))
            {
                Result.Append("\"").Append(table.TableSchema).Append("\".");
            }

            Result.Append("\"").Append(table.TableName).Append("\"");
        }

        public void Visit(Table table)
        {
            if (ExpectsFqn)
            {
                AppendFullyQuallifiedTableName(table);
                ExpectsFqn = false;

                if (State == CoarseState.FromAndJoins && !string.IsNullOrWhiteSpace(table.TableAlias))
                {
                    Result.Append(" AS ").Append(table.TableAlias);
                }
            }
            else if (string.IsNullOrWhiteSpace(table.TableAlias))
            {
                AppendFullyQuallifiedTableName(table);
            }
            else
            {
                Result.Append(table.TableAlias);
            }
        }

        public void Visit(ArrayAggPart arrayAggPart)
        {
            Result.Append("array_agg(");
            arrayAggPart.Over.Accept(this);
            Result.Append(")");
        }

        public void Visit(ToTsVectorPart toTsVector)
        {
            Result.Append("to_tsvector('").Append(toTsVector.Regconfig).Append("', ");
            toTsVector.Over.Accept(this);
            Result.Append(")");
        }

        public void Visit(ToTsQueryPart toTsQuery)
        {
            Result.Append("to_tsquery('").Append(toTsQuery.Regconfig).Append("', ");
            toTsQuery.Over.Accept(this);
            Result.Append(")");
        }

        public void Visit(FullTextSearchMatchPart fullTextSearchMatch)
        {
            HandleOperation(fullTextSearchMatch.Vector, fullTextSearchMatch.Query, "@@");
        }

        public void Visit(InPart inPart)
        {
            inPart.Left.Accept(this);
            Result.Append(" = ANY (");
            inPart.Right.Accept(this);
            Result.Append(")");
        }

        public void Visit<T>(IArrayParameter<T> arrayParam)
        {
            if (State == CoarseState.ExtraParameters)
            {
                Result.AddParameter(arrayParam.Name, arrayParam.Value);
            }
            else
            {
                Result.Append("@").Append(arrayParam.Name).Append(" ").AddParameter(arrayParam.Name, arrayParam.Value);
            }
        }

        public void Visit(IntervalPart interval)
        {
            Result.Append("INTERVAL ");

            interval.Part.Accept(this);

            switch (interval.IntervalType)
            {
                case IntervalType.Day:
                    Result.Append("DAY ");
                    break;
                case IntervalType.Month:
                    Result.Append("MONTH ");
                    break;
                case IntervalType.Week:
                    Result.Append("WEEK ");
                    break;
                case IntervalType.Year:
                    Result.Append("YEAR ");
                    break;
                case IntervalType.Hour:
                    Result.Append("HOUR ");
                    break;
                case IntervalType.Minute:
                    Result.Append("MINUTE ");
                    break;
                case IntervalType.Second:
                    Result.Append("SECOND ");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}