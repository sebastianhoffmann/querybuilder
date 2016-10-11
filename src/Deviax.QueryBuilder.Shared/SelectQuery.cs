using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;
using System;
using System.Linq;

namespace Deviax.QueryBuilder
{
    public class SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> : BaseSelectQuery<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>>
        where TTable : Table
        where TTable2 : Table
        where TTable3 : Table
        where TTable4 : Table
        where TTable5 : Table
        where TTable6 : Table
        where TTable7 : Table
        where TTable8 : Table
        where TTable9 : Table
        where TTable10 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        internal readonly TTable3 Table3;
        internal readonly TTable4 Table4;
        internal readonly TTable5 Table5;
        internal readonly TTable6 Table6;
        internal readonly TTable7 Table7;
        internal readonly TTable8 Table8;
        internal readonly TTable9 Table9;
        internal readonly TTable10 Table10;
        public SelectQuery(TTable t, TTable2 t2, TTable3 t3, TTable4 t4, TTable5 t5, TTable6 t6, TTable7 t7, TTable8 t8, TTable9 t9, TTable10 t10)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
            Table7 = t7;
            Table8 = t8;
            Table9 = t9;
            Table10 = t10;
        }

        internal SelectQuery(
            TTable t1,
            TTable2 t2,
            TTable3 t3,
            TTable4 t4,
            TTable5 t5,
            TTable6 t6,
            TTable7 t7,
            TTable8 t8,
            TTable9 t9,
            TTable10 t10,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
            Table7 = t7;
            Table8 = t8;
            Table9 = t9;
            Table10 = t10;
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> Where(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> Select(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> GroupBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> Having(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> OrderBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, Table10)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> New(SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(t.Table1, t.Table2, t.Table3, t.Table4, t.Table5, t.Table6, t.Table7, t.Table8, t.Table9, t.Table10, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public class SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> : BaseSelectQuery<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>>
        where TTable : Table
        where TTable2 : Table
        where TTable3 : Table
        where TTable4 : Table
        where TTable5 : Table
        where TTable6 : Table
        where TTable7 : Table
        where TTable8 : Table
        where TTable9 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        internal readonly TTable3 Table3;
        internal readonly TTable4 Table4;
        internal readonly TTable5 Table5;
        internal readonly TTable6 Table6;
        internal readonly TTable7 Table7;
        internal readonly TTable8 Table8;
        internal readonly TTable9 Table9;
        public SelectQuery(TTable t, TTable2 t2, TTable3 t3, TTable4 t4, TTable5 t5, TTable6 t6, TTable7 t7, TTable8 t8, TTable9 t9)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
            Table7 = t7;
            Table8 = t8;
            Table9 = t9;
        }

        internal SelectQuery(
            TTable t1,
            TTable2 t2,
            TTable3 t3,
            TTable4 t4,
            TTable5 t5,
            TTable6 t6,
            TTable7 t7,
            TTable8 t8,
            TTable9 t9,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
            Table7 = t7;
            Table8 = t8;
            Table9 = t9;
        }


        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>, TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> InnerJoin<TTable10>(TTable10 p) 
            where TTable10 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>, 
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(
                Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(
                    Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, p,
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>, TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> LeftJoin<TTable10>(TTable10 p)
            where TTable10 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(
                Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(
                    Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, p,
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>, TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> RightJoin<TTable10>(TTable10 p)
            where TTable10 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(
                Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10>(
                    Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9, p,
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> Where(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> Select(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> GroupBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> Having(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> OrderBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, Table9)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> New(SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(t.Table1, t.Table2, t.Table3, t.Table4, t.Table5, t.Table6, t.Table7, t.Table8, t.Table9, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public class SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> 
        : BaseSelectQuery<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>>
        where TTable : Table
        where TTable2 : Table
        where TTable3 : Table
        where TTable4 : Table
        where TTable5 : Table
        where TTable6 : Table
        where TTable7 : Table
        where TTable8 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        internal readonly TTable3 Table3;
        internal readonly TTable4 Table4;
        internal readonly TTable5 Table5;
        internal readonly TTable6 Table6;
        internal readonly TTable7 Table7;
        internal readonly TTable8 Table8;
        public SelectQuery(TTable t, TTable2 t2, TTable3 t3, TTable4 t4, TTable5 t5, TTable6 t6, TTable7 t7, TTable8 t8)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
            Table7 = t7;
            Table8 = t8;
        }

        internal SelectQuery(
            TTable t1,
            TTable2 t2,
            TTable3 t3,
            TTable4 t4,
            TTable5 t5,
            TTable6 t6,
            TTable7 t7,
            TTable8 t8,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
            Table7 = t7;
            Table8 = t8;
        }


        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>, 
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> InnerJoin<TTable9>(TTable9 p)
            where TTable9 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(
                Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(
                    Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, p,
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>, 
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> LeftJoin<TTable9>(TTable9 p)
            where TTable9 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(
                Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(
                    Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, p,
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>, 
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9> RightJoin<TTable9>(TTable9 p)
            where TTable9 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(
                Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>(
                    Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8, p,
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> Where(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> Select(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> GroupBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> Having(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> OrderBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7, Table8)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> New(SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(t.Table1, t.Table2, t.Table3, t.Table4, t.Table5, t.Table6, t.Table7, t.Table8, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public class SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>
        : BaseSelectQuery<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>>
        where TTable : Table
        where TTable2 : Table
        where TTable3 : Table
        where TTable4 : Table
        where TTable5 : Table
        where TTable6 : Table
        where TTable7 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        internal readonly TTable3 Table3;
        internal readonly TTable4 Table4;
        internal readonly TTable5 Table5;
        internal readonly TTable6 Table6;
        internal readonly TTable7 Table7;
        public SelectQuery(TTable t, TTable2 t2, TTable3 t3, TTable4 t4, TTable5 t5, TTable6 t6, TTable7 t7)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
            Table7 = t7;
        }

        internal SelectQuery(
            TTable t1,
            TTable2 t2,
            TTable3 t3,
            TTable4 t4,
            TTable5 t5,
            TTable6 t6,
            TTable7 t7,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
            Table7 = t7;
        }
        
        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>,
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> InnerJoin<TTable8>(TTable8 p)
            where TTable8 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(
                Table1, Table2, Table3, Table4, Table5, Table6, Table7, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(
                    Table1, Table2, Table3, Table4, Table5, Table6, Table7, p,
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>,
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> LeftJoin<TTable8>(TTable8 p)
            where TTable8 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(
                Table1, Table2, Table3, Table4, Table5, Table6, Table7, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(
                    Table1, Table2, Table3, Table4, Table5, Table6, Table7, p,
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>,
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8> RightJoin<TTable8>(TTable8 p)
            where TTable8 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(
                Table1, Table2, Table3, Table4, Table5, Table6, Table7, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>(
                    Table1, Table2, Table3, Table4, Table5, Table6, Table7, p,
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> Where(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(Table1, Table2, Table3, Table4, Table5, Table6, Table7,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> Select(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(Table1, Table2, Table3, Table4, Table5, Table6, Table7,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> GroupBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(Table1, Table2, Table3, Table4, Table5, Table6, Table7,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> Having(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(Table1, Table2, Table3, Table4, Table5, Table6, Table7,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> OrderBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(Table1, Table2, Table3, Table4, Table5, Table6, Table7,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6, Table7)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> New(SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(t.Table1, t.Table2, t.Table3, t.Table4, t.Table5, t.Table6, t.Table7, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public class SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>
        : BaseSelectQuery<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>>
        where TTable : Table
        where TTable2 : Table
        where TTable3 : Table
        where TTable4 : Table
        where TTable5 : Table
        where TTable6 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        internal readonly TTable3 Table3;
        internal readonly TTable4 Table4;
        internal readonly TTable5 Table5;
        internal readonly TTable6 Table6;
        public SelectQuery(TTable t, TTable2 t2, TTable3 t3, TTable4 t4, TTable5 t5, TTable6 t6)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
        }

        internal SelectQuery(
            TTable t1,
            TTable2 t2,
            TTable3 t3,
            TTable4 t4,
            TTable5 t5,
            TTable6 t6,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
            Table6 = t6;
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>,
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> InnerJoin<TTable7>(TTable7 p)
            where TTable7 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(
                Table1, Table2, Table3, Table4, Table5, Table6, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(
                    Table1, Table2, Table3, Table4, Table5, Table6, p,
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>,
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> LeftJoin<TTable7>(TTable7 p)
            where TTable7 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(
                Table1, Table2, Table3, Table4, Table5, Table6, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(
                    Table1, Table2, Table3, Table4, Table5, Table6, p,
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>,
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7> RightJoin<TTable7>(TTable7 p)
            where TTable7 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(
                Table1, Table2, Table3, Table4, Table5, Table6, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>(
                    Table1, Table2, Table3, Table4, Table5, Table6, p,
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6> Where(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(Table1, Table2, Table3, Table4, Table5, Table6,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6> Select(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(Table1, Table2, Table3, Table4, Table5, Table6,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6> GroupBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(Table1, Table2, Table3, Table4, Table5, Table6,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6> Having(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(Table1, Table2, Table3, Table4, Table5, Table6,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6> OrderBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(Table1, Table2, Table3, Table4, Table5, Table6,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5, Table6)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6> New(SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(t.Table1, t.Table2, t.Table3, t.Table4, t.Table5, t.Table6, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public class SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>
        : BaseSelectQuery<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>>
        where TTable : Table
        where TTable2 : Table
        where TTable3 : Table
        where TTable4 : Table
        where TTable5 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        internal readonly TTable3 Table3;
        internal readonly TTable4 Table4;
        internal readonly TTable5 Table5;
        public SelectQuery(TTable t, TTable2 t2, TTable3 t3, TTable4 t4, TTable5 t5)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
        }

        internal SelectQuery(
            TTable t1,
            TTable2 t2,
            TTable3 t3,
            TTable4 t4,
            TTable5 t5,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
            Table5 = t5;
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>,
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6> InnerJoin<TTable6>(TTable6 p)
            where TTable6 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(
                Table1, Table2, Table3, Table4, Table5, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(
                    Table1, Table2, Table3, Table4, Table5, p,
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>,
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6> LeftJoin<TTable6>(TTable6 p)
            where TTable6 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(
                Table1, Table2, Table3, Table4, Table5, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(
                    Table1, Table2, Table3, Table4, Table5, p,
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>,
            TTable, TTable2, TTable3, TTable4, TTable5, TTable6> RightJoin<TTable6>(TTable6 p)
            where TTable6 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>,
                TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(
                Table1, Table2, Table3, Table4, Table5, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5, TTable6>(
                    Table1, Table2, Table3, Table4, Table5, p,
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5> Where(params Func<TTable, TTable2, TTable3, TTable4, TTable5, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>(Table1, Table2, Table3, Table4, Table5,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5> Select(params Func<TTable, TTable2, TTable3, TTable4, TTable5, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>(Table1, Table2, Table3, Table4, Table5,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5> GroupBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>(Table1, Table2, Table3, Table4, Table5,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5> Having(params Func<TTable, TTable2, TTable3, TTable4, TTable5, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>(Table1, Table2, Table3, Table4, Table5,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5> OrderBy(params Func<TTable, TTable2, TTable3, TTable4, TTable5, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>(Table1, Table2, Table3, Table4, Table5,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1, Table2, Table3, Table4, Table5)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5> New(SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>(t.Table1, t.Table2, t.Table3, t.Table4, t.Table5, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public class SelectQuery<TTable, TTable2, TTable3, TTable4>
    : BaseSelectQuery<SelectQuery<TTable, TTable2, TTable3, TTable4>>
    where TTable : Table
    where TTable2 : Table
    where TTable3 : Table
    where TTable4 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        internal readonly TTable3 Table3;
        internal readonly TTable4 Table4;
        public SelectQuery(TTable t, TTable2 t2, TTable3 t3, TTable4 t4)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
        }

        internal SelectQuery(
            TTable t1,
            TTable2 t2,
            TTable3 t3,
            TTable4 t4,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
            Table3 = t3;
            Table4 = t4;
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>,
            TTable, TTable2, TTable3, TTable4, TTable5> InnerJoin<TTable5>(TTable5 p)
            where TTable5 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>,
                TTable, TTable2, TTable3, TTable4, TTable5>(
                Table1, Table2, Table3, Table4, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>(
                    Table1, Table2, Table3, Table4, p,
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>,
            TTable, TTable2, TTable3, TTable4, TTable5> LeftJoin<TTable5>(TTable5 p)
            where TTable5 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>,
                TTable, TTable2, TTable3, TTable4, TTable5>(
                Table1, Table2, Table3, Table4, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>(
                    Table1, Table2, Table3, Table4, p,
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>,
            TTable, TTable2, TTable3, TTable4, TTable5> RightJoin<TTable5>(TTable5 p)
            where TTable5 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>,
                TTable, TTable2, TTable3, TTable4, TTable5>(
                Table1, Table2, Table3, Table4, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4, TTable5>(
                    Table1, Table2, Table3, Table4, p,
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4> Where(params Func<TTable, TTable2, TTable3, TTable4, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4>(Table1, Table2, Table3, Table4,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1, Table2, Table3, Table4)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4> Select(params Func<TTable, TTable2, TTable3, TTable4, IPart>[] parts)
        { 
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4>(Table1, Table2, Table3, Table4,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1, Table2, Table3, Table4)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4> GroupBy(params Func<TTable, TTable2, TTable3, TTable4, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4>(Table1, Table2, Table3, Table4,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1, Table2, Table3, Table4)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4> Having(params Func<TTable, TTable2, TTable3, TTable4, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4>(Table1, Table2, Table3, Table4,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1, Table2, Table3, Table4)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3, TTable4> OrderBy(params Func<TTable, TTable2, TTable3, TTable4, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3, TTable4>(Table1, Table2, Table3, Table4,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1, Table2, Table3, Table4)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable, TTable2, TTable3, TTable4> New(SelectQuery<TTable, TTable2, TTable3, TTable4> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable, TTable2, TTable3, TTable4>(t.Table1, t.Table2, t.Table3, t.Table4, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public class SelectQuery<TTable, TTable2, TTable3>
    : BaseSelectQuery<SelectQuery<TTable, TTable2, TTable3>>
    where TTable : Table
    where TTable2 : Table
    where TTable3 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        internal readonly TTable3 Table3;
        public SelectQuery(TTable t, TTable2 t2, TTable3 t3)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
            Table3 = t3;
        }

        internal SelectQuery(
            TTable t1,
            TTable2 t2,
            TTable3 t3,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
            Table3 = t3;
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4>,
            TTable, TTable2, TTable3, TTable4> InnerJoin<TTable4>(TTable4 p)
            where TTable4 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4>,
                TTable, TTable2, TTable3, TTable4>(
                Table1, Table2, Table3, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4>(
                    Table1, Table2, Table3, p,
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4>,
            TTable, TTable2, TTable3, TTable4> LeftJoin<TTable4>(TTable4 p)
            where TTable4 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4>,
                TTable, TTable2, TTable3, TTable4>(
                Table1, Table2, Table3, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4>(
                    Table1, Table2, Table3, p,
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4>,
            TTable, TTable2, TTable3, TTable4> RightJoin<TTable4>(TTable4 p)
            where TTable4 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3, TTable4>,
                TTable, TTable2, TTable3, TTable4>(
                Table1, Table2, Table3, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3, TTable4>(
                    Table1, Table2, Table3, p,
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3> Where(params Func<TTable, TTable2, TTable3, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3>(Table1, Table2, Table3,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1, Table2, Table3)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3> Select(params Func<TTable, TTable2, TTable3, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3>(Table1, Table2, Table3,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1, Table2, Table3)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3> GroupBy(params Func<TTable, TTable2, TTable3, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3>(Table1, Table2, Table3,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1, Table2, Table3)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3> Having(params Func<TTable, TTable2, TTable3, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3>(Table1, Table2, Table3,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1, Table2, Table3)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2, TTable3> OrderBy(params Func<TTable, TTable2, TTable3, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2, TTable3>(Table1, Table2, Table3,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1, Table2, Table3)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable, TTable2, TTable3> New(SelectQuery<TTable, TTable2, TTable3> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable, TTable2, TTable3>(t.Table1, t.Table2, t.Table3, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public class SelectQuery<TTable, TTable2>
    : BaseSelectQuery<SelectQuery<TTable, TTable2>>
    where TTable : Table
    where TTable2 : Table
    {
        internal readonly TTable Table1;
        internal readonly TTable2 Table2;
        public SelectQuery(TTable t, TTable2 t2)
            : base(t)
        {
            Table1 = t;
            Table2 = t2;
        }

        internal SelectQuery(
            TTable t1,
            TTable2 t2,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
            Table2 = t2;
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3>,
            TTable, TTable2, TTable3> InnerJoin<TTable3>(TTable3 p)
            where TTable3 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3>,
                TTable, TTable2, TTable3>(
                Table1, Table2, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3>(
                    Table1, Table2, p,
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3>,
            TTable, TTable2, TTable3> LeftJoin<TTable3>(TTable3 p)
            where TTable3 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3>,
                TTable, TTable2, TTable3>(
                Table1, Table2, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3>(
                    Table1, Table2, p,
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2, TTable3>,
            TTable, TTable2, TTable3> RightJoin<TTable3>(TTable3 p)
            where TTable3 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2, TTable3>,
                TTable, TTable2, TTable3>(
                Table1, Table2, p, parts =>
                new SelectQuery<TTable, TTable2, TTable3>(
                    Table1, Table2, p,
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2> Where(params Func<TTable, TTable2, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2>(Table1, Table2,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1, Table2)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2> Select(params Func<TTable, TTable2, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2>(Table1, Table2,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1, Table2)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2> GroupBy(params Func<TTable, TTable2, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2>(Table1, Table2,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1, Table2)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2> Having(params Func<TTable, TTable2, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2>(Table1, Table2,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1, Table2)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable, TTable2> OrderBy(params Func<TTable, TTable2, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable, TTable2>(Table1, Table2,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1, Table2)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable, TTable2> New(SelectQuery<TTable, TTable2> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable, TTable2>(t.Table1, t.Table2, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public class SelectQuery<TTable>
    : BaseSelectQuery<SelectQuery<TTable>>
    where TTable : Table
    {
        internal readonly TTable Table1;
        public SelectQuery(TTable t)
            : base(t)
        {
            Table1 = t;
        }

        internal SelectQuery(
            TTable t1,
            IFromPart from,
           List<JoinPart> joins,
           List<IPart> selectParts,
           List<IBooleanPart> whereParts,
           List<IBooleanPart> havingParts,
           List<IPart> groupByParts,
           List<IOrderPart> orderByParts,
           LimitOffsetPart limitOffsetPart,
           List<IPart> extraParameters
           ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
            Table1 = t1;
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2>,
            TTable, TTable2> InnerJoin<TTable2>(TTable2 p)
            where TTable2 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2>,
                TTable, TTable2>(
                Table1, p, parts =>
                new SelectQuery<TTable, TTable2>(
                    Table1, p,
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2>,
            TTable, TTable2> LeftJoin<TTable2>(TTable2 p)
            where TTable2 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2>,
                TTable, TTable2>(
                Table1, p, parts =>
                new SelectQuery<TTable, TTable2>(
                    Table1, p,
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery<TTable, TTable2>,
            TTable, TTable2> RightJoin<TTable2>(TTable2 p)
            where TTable2 : Table
        {
            return new JoinBuilder<SelectQuery<TTable, TTable2>,
                TTable, TTable2>(
                Table1, p, parts =>
                new SelectQuery<TTable, TTable2>(
                    Table1, p,
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart, ExtraParameters
                )
            );
        }

        [Pure]
        public SelectQuery<TTable> Where(params Func<TTable, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable>(Table1,
                From, Joins, SelectParts, With(WhereParts, parts.Select(p => p(Table1)).ToArray()), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable> Select(params Func<TTable, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable>(Table1,
                From, Joins, With(SelectParts, parts.Select(p => p(Table1)).ToArray()), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable> GroupBy(params Func<TTable, IPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable>(Table1,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts.Select(p => p(Table1)).ToArray()), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable> Having(params Func<TTable, IBooleanPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable>(Table1,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts.Select(p => p(Table1)).ToArray()),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public SelectQuery<TTable> OrderBy(params Func<TTable, IOrderPart>[] parts)
        {
            return parts.Length == 0 ? this : new SelectQuery<TTable>(Table1,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts.Select(p => p(Table1)).ToArray()), LimitOffsetPart,
                ExtraParameters
            );
        }

        protected override SelectQuery<TTable> New(SelectQuery<TTable> t, IFromPart from, List<JoinPart> joins, List<IPart> selectParts, List<IBooleanPart> whereParts, List<IBooleanPart> havingParts, List<IPart> groupByParts, List<IOrderPart> orderByParts, LimitOffsetPart limitOffsetPart, List<IPart> extraParameters)
        {
            return new SelectQuery<TTable>(t.Table1, from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        }
    }

    public abstract class BaseSelectQuery : Part
    {
        internal readonly IFromPart From;
        internal readonly List<IPart> GroupByParts;
        internal readonly List<IBooleanPart> HavingParts;
        internal readonly List<JoinPart> Joins;
        internal readonly List<IOrderPart> OrderByParts;
        internal readonly List<IPart> SelectParts;
        internal readonly List<IBooleanPart> WhereParts;
        internal readonly LimitOffsetPart LimitOffsetPart;
        internal readonly List<IPart> ExtraParameters;

        protected BaseSelectQuery(IFromPart from,
            List<JoinPart> joins,
            List<IPart> selectParts,
            List<IBooleanPart> whereParts,
            List<IBooleanPart> havingParts,
            List<IPart> groupByParts,
            List<IOrderPart> orderByParts,
            LimitOffsetPart limitOffsetPart,
            List<IPart> extraParameters
            )
        {
            From = from;
            Joins = joins;
            SelectParts = selectParts;
            WhereParts = whereParts;
            HavingParts = havingParts;
            GroupByParts = groupByParts;
            OrderByParts = orderByParts;
            LimitOffsetPart = limitOffsetPart;
            ExtraParameters = extraParameters;
        }

        protected BaseSelectQuery(IFromPart from)
        {
            From = from;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public async Task<List<T>> ToList<T>(DbConnection con, DbTransaction tx = null) where T : new()
        {
            return await QueryExecutor.DefaultExecutor.ToList<T>(this, con, tx).ConfigureAwait(false);
        }

        public async Task<T> FirstOrDefault<T>(DbConnection con, DbTransaction tx = null) where T : new()
        {
            return await QueryExecutor.DefaultExecutor.FirstOrDefault<T>(this, con, tx).ConfigureAwait(false);
        }

        public new AliasedSelectQuery As(string alias) => new AliasedSelectQuery(alias, this);

        public async Task<T> ScalarResult<T>(DbConnection con, DbTransaction tx = null)
        {
            return await QueryExecutor.DefaultExecutor.ScalarResult<T>(this, con, tx).ConfigureAwait(false);
        }

        public async Task<List<T>> ScalarList<T>(DbConnection con, DbTransaction tx = null)
        {
            return await QueryExecutor.DefaultExecutor.ScalarListResult<T>(this, con, tx).ConfigureAwait(false);
        }

        public string StringRepresentation => ToString();

        public override string ToString()
        {
            return QueryExecutor.DefaultExecutor.ToQueryText(this);
        }
    }

    public abstract class BaseSelectQuery<TQ>  : BaseSelectQuery where TQ : BaseSelectQuery<TQ>
    {
        protected BaseSelectQuery(IFromPart from,
            List<JoinPart> joins,
            List<IPart> selectParts,
            List<IBooleanPart> whereParts,
            List<IBooleanPart> havingParts,
            List<IPart> groupByParts,
            List<IOrderPart> orderByParts,
            LimitOffsetPart limitOffsetPart,
            List<IPart> extraParameters
            ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
        }

        protected abstract TQ New(
            TQ t,
            IFromPart from,
            List<JoinPart> joins,
            List<IPart> selectParts,
            List<IBooleanPart> whereParts,
            List<IBooleanPart> havingParts,
            List<IPart> groupByParts,
            List<IOrderPart> orderByParts,
            LimitOffsetPart limitOffsetPart,
            List<IPart> extraParameters
        );

        protected BaseSelectQuery(IFromPart from) : base(from)
        {
        }

        protected static List<T> With<T>(IReadOnlyCollection<T> existing, params T[] toAdd)
        {
            if (existing == null)
                return new List<T>(toAdd);

            var l = new List<T>(existing.Count + toAdd.Length);
            l.AddRange(existing);
            l.AddRange(toAdd);
            return l;
        }

        [Pure]
        public TQ Where(params IBooleanPart[] parts)
        {
            return parts.Length == 0 ? (TQ)this : New((TQ)this,
                From, Joins, SelectParts, With(WhereParts, parts), HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public TQ Having(params IBooleanPart[] parts)
        {
            return parts.Length == 0 ? (TQ)this : New((TQ)this,
                From, Joins, SelectParts, WhereParts, With(HavingParts, parts),
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public TQ OrderBy(params IOrderPart[] parts)
        {
            return parts.Length == 0 ? (TQ)this : New((TQ)this,
                From, Joins, SelectParts, WhereParts, HavingParts,
                GroupByParts, With(OrderByParts, parts), LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public TQ GroupBy(params IPart[] parts)
        {
            return parts.Length == 0 ? (TQ)this : New((TQ)this,
                From, Joins, SelectParts, WhereParts, HavingParts,
                With(GroupByParts, parts), OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public TQ Select(params IPart[] parts)
        {
            return parts.Length == 0 ? (TQ)this : New((TQ)this,
                From, Joins, With(SelectParts, parts), WhereParts, HavingParts,
                GroupByParts, OrderByParts, LimitOffsetPart,
                ExtraParameters
            );
        }

        [Pure]
        public TQ Limit(int i) => New((TQ)this,
            From, Joins, SelectParts, WhereParts, HavingParts,
            GroupByParts, OrderByParts, new LimitOffsetPart(i, LimitOffsetPart?.Offset),
                ExtraParameters
        );

        [Pure]
        public TQ Offset(int i) => New((TQ)this,
            From, Joins, SelectParts, WhereParts, HavingParts,
            GroupByParts, OrderByParts, new LimitOffsetPart(LimitOffsetPart?.Limit, i),
                ExtraParameters
        );

        [Pure]
        public TQ Limit(int limit, int offset) => New((TQ)this,
            From, Joins, SelectParts, WhereParts, HavingParts,
            GroupByParts, OrderByParts, new LimitOffsetPart(limit, offset),
                ExtraParameters
        );

        [Pure]
        public TQ WithExtraParameter(params IPart[] parameters) => New((TQ)this,
            From, Joins, SelectParts, WhereParts, HavingParts,
            GroupByParts, OrderByParts, LimitOffsetPart,
            With(ExtraParameters, parameters)
        );
    }

    public class SelectQuery : BaseSelectQuery<SelectQuery>
    {
        internal SelectQuery(IFromPart from,
            List<JoinPart> joins,
            List<IPart> selectParts,
            List<IBooleanPart> whereParts,
            List<IBooleanPart> havingParts,
            List<IPart> groupByParts,
            List<IOrderPart> orderByParts,
            LimitOffsetPart limitOffsetPart,
            List<IPart> extraParameters 
            ) : base(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters)
        {
        }

        public SelectQuery(IFromPart from) : base(from)
        {
            
        }

        protected override SelectQuery New(
            SelectQuery t,
            IFromPart from,
            List<JoinPart> joins,
            List<IPart> selectParts,
            List<IBooleanPart> whereParts,
            List<IBooleanPart> havingParts,
            List<IPart> groupByParts,
            List<IOrderPart> orderByParts,
            LimitOffsetPart limitOffsetPart,
            List<IPart> extraParameters
        ) => new SelectQuery(from, joins, selectParts, whereParts, havingParts, groupByParts, orderByParts, limitOffsetPart, extraParameters);
        
        [Pure]
        public JoinBuilder<SelectQuery> InnerJoin(IFromPart p)
        {
            return new JoinBuilder<SelectQuery>(parts => 
                new SelectQuery(
                    From, With(Joins, new InnerJoinPart(p, parts)), SelectParts, 
                    WhereParts, HavingParts, GroupByParts, OrderByParts, 
                    LimitOffsetPart,
                ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery> LeftJoin(IFromPart p)
        {
            return new JoinBuilder<SelectQuery>(parts =>
                new SelectQuery(
                    From, With(Joins, new LeftJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart,
                ExtraParameters
                )
            );
        }

        [Pure]
        public JoinBuilder<SelectQuery> RightJoin(IFromPart p)
        {
            return new JoinBuilder<SelectQuery>(parts =>
                new SelectQuery(
                    From, With(Joins, new RightJoinPart(p, parts)), SelectParts,
                    WhereParts, HavingParts, GroupByParts, OrderByParts,
                    LimitOffsetPart,
                ExtraParameters
                )
            );
        }
    }

    public class LimitOffsetPart
    {
        public readonly int? Limit;
        public readonly int? Offset;
        public LimitOffsetPart(int? limit, int? offset)
        {
            Limit = limit;
            Offset = offset;
        }

        public void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class AliasedSelectQuery : Part, IFromPart, IAliasPart
    {
        public AliasedSelectQuery(string alias, BaseSelectQuery selectQuery)
        {
            Name = alias;
            Aliased = selectQuery;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public string Name { get; }
        public IPart Aliased { get; }
    }
}