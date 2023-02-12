using System;
using System.Diagnostics;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Parts.Aggregation;
using Deviax.QueryBuilder.Parts.FullTextSearch;

namespace Deviax.QueryBuilder.Visitors
{
    public abstract partial class BaseVisitor
    {
        protected bool LimitWithoutOffsetAtStart => false;

        public void Visit(StringConcatenation sc) => HandleOperation(sc.Left, sc.Right, "|| ");

        public void Visit(LikePart lp) => HandleOperation(lp.Left, lp.Right, lp.Mode == LikeMode.CaseSensitive ? "LIKE " : "ILIKE ");

        public void Visit(IField field)
        {
            if (NoTableName)
            {
                Result.Append("\"").Append(field.Name).Append("\" ");
                NoTableName = false; // TODO: rework this
            }
            else
            {
                field.Table.Accept(this);
                Result.Append(".\"").Append(field.Name).Append("\" ");
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
            //TODO: probably incorrect?
            aliased.Aliased.Accept(this);

            if (State == CoarseState.Select || ExpectsFqn)
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

        public void Visit(ArrayAggPart arrayAggPart)
        {
            Result.Append("array_agg(");
            arrayAggPart.Over.Accept(this);
            Result.Append(")");

            if (arrayAggPart.FilterPart != null)
            {
                Result.Append("FILTER (WHERE ");
                arrayAggPart.FilterPart.Accept(this);
                Result.Append(")");
            }
        }

        public void Visit(MaxPart max)
        {
            Result.Append("MAX(");
            max.Over.Accept(this);
            Result.Append(") ");

            if (max.FilterPart != null)
            {
                Result.Append("FILTER (WHERE ");
                max.FilterPart.Accept(this);
                Result.Append(")");
            }
        }

        public void Visit(AbsPart absPart)
        {
            Result.Append("ABS(");
            absPart.Over.Accept(this);
            Result.Append(") ");

            if (absPart.FilterPart != null)
            {
                Result.Append("FILTER (WHERE ");
                absPart.FilterPart.Accept(this);
                Result.Append(")");
            }
        }

        public void Visit(MinPart min)
        {
            Result.Append("MIN(");
            min.Over.Accept(this);
            Result.Append(") ");

            if (min.FilterPart != null)
            {
                Result.Append("FILTER (WHERE ");
                min.FilterPart.Accept(this);
                Result.Append(")");
            }
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

        public void Visit(ContainsPart containsPart)
        {
            containsPart.Left.Accept(this);
            Result.Append(" = ANY (");
            containsPart.Right.Accept(this);
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
                Result.Append("@").Append(arrayParam.Name).Append(" ").AddParameter(arrayParam);
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

        public void Visit(AgePart agePart)
        {
            Result.Append("AGE(");
            agePart.Of.Accept(this);
            Result.Append(") ");
        }

        public void Visit(ExtractPart extractPart)
        {
            string arg;
            switch (extractPart.What)
            {
                case Extractable.Day:
                    arg = "day";
                    break;
                case Extractable.Month:
                    arg = "month";
                    break;
                case Extractable.Year:
                    arg = "year";
                    break;

                default:
                    throw new ArgumentException();
            }

            Result.Append("EXTRACT(").Append(arg).Append(" FROM ");
            extractPart.From.Accept(this);
            Result.Append(") ");
        }

        public void Visit(CastPart castPart)
        {
            castPart.What.Accept(this);
            Result.Append("::").Append(castPart.TargetType).Append(" ");
        }

        public abstract void Visit(RowNumberPart rowNumberPart);
        public abstract void Visit(PartitionPart partitionPart);

        public void Visit(UnnestTable unnestTable)
        {
            if (ExpectsFqn)
            {
                Result.Append(
                    "UNNEST ("
                );

                unnestTable.Parameters[0].Accept(this);
                for (int i = 1; i < unnestTable.Parameters.Count; i++)
                {
                    Result.Append(",");
                    unnestTable.Parameters[i].Accept(this);
                }

                Debug.Assert(unnestTable.TableAlias != null, "unnestTable.TableAlias != null");
                Result.Append(") AS ").Append(unnestTable.TableAlias).Append("(").Append(unnestTable.Parameters[0].Name);
                for (int i = 1; i < unnestTable.Parameters.Count; i++)
                {
                    Result.Append(",").Append(unnestTable.Parameters[i].Name);
                }

                Result.Append(") ");
            }
            else
            {
                Debug.Assert(unnestTable.TableAlias != null, "unnestTable.TableAlias != null");
                Result.Append(unnestTable.TableAlias);
            }
        }

        public void Visit(ArrayOverlapPart arrayOverlapPart)
        {
            arrayOverlapPart.Left.Accept(this);
            Result.Append("&& ");
            arrayOverlapPart.Right.Accept(this);
        }

        public void Visit(MatchesRegexPart matchesRegexPart)
        {
            HandleOperation(matchesRegexPart.Left, matchesRegexPart.Right, " ~ ");
        }

        private void AppendFullyQualifiedTableName(Table table)
        {
            if (!string.IsNullOrWhiteSpace(table.TableSchema))
            {
                Result.Append("\"").Append(table.TableSchema).Append("\".");
            }
            
            Debug.Assert(table.TableName != null, "table.TableName != null");
            Result.Append("\"").Append(table.TableName).Append("\"");
        }
    }
}