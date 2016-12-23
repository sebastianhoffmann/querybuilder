using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public partial interface INodeVisitor
    {
        void Visit(AndPart and);
        void Visit(OrPart or);
        void Visit(EqPart eq);
        void Visit(SetFieldPart sfp);
        void Visit(BaseDeleteQuery deleteQuery);
        void Visit(StringConcatenation sc);
        void Visit(LikePart lp);

        void Visit<T>(IParameter<T> parameter);
        void Visit(IField field);
        void Visit(IAliasPart aliased);
        void Visit(AliasedSelectQuery aliased);
        void Visit(InnerJoinPart innerJoinPart);
        void Visit(RawSql rawSql);
        void Visit(GtePart gte);
        void Visit(LtePart lte);
        void Visit(GtPart gt);
        void Visit(LtPart lt);
        void Visit(IsNotNullPart notNull);
        void Visit(DescOrdering desc);
        void Visit(AscOrdering asc);
        void Visit(IsNullPart isNull);
        void Visit(NeqPart neq);
        void Visit(Table table);
        void Visit(BaseSelectQuery query);
        void Visit(LimitOffsetPart limitOffset);
        void Visit(InPart inPart);
        void Visit<T>(IArrayParameter<T> arrayParam);
        void Visit(IntervalPart interval);
        void Visit(BetweenPart betweenPart);
        void Visit(IsFalsePart part);
        void Visit(MaxPart max);
        void Visit(MinPart min);
        void Visit<T>(Literal<T> literal);
        void Visit(IsTruePart truePart);
        void Visit(PlusPart plusPart);
        void Visit(MinusPart minusPart);
        void Visit(MulPart mulPart);
        void Visit(DivPart divPart);
        void Visit(ModPart modPart);
        void Visit(LowerPart lowerPart);
        void Visit(RightJoinPart rightJoinPart);
        void Visit(LeftJoinPart leftJoinPart);
        void Visit(SumPart sumPart);
        void Visit(CountPart countPart);
        void Visit(CoalescePart coalesce);
        void Visit(CasePart casePart);
        void Visit(DistinctPart distinctPart);
        void Visit(AbsPart absPart);
    }
}