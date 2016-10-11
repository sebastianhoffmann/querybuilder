using System;
using Deviax.QueryBuilder.Visitors;
using System.Linq;

namespace Deviax.QueryBuilder.Parts
{
    public abstract class JoinPart
    {
        public abstract void Accept(INodeVisitor visitor);
    }

    public sealed class JoinBuilder<T>
    {
        private readonly Func<IBooleanPart[], T> _apply;

        public JoinBuilder(Func<IBooleanPart[], T> f)
        {
            _apply = f;
        }

        public T On(params IBooleanPart[] p) => _apply(p);
    }

    public sealed class JoinBuilder<T, TTable> 
        where TTable : Table 
    {
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        public JoinBuilder(TTable table, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
        }

        public T On(params Func<TTable, IBooleanPart>[] p) 
            => _apply(p.Select(f => f(_table)).ToArray());
    }

    public sealed class JoinBuilder<T, TTable, TTable2>
     where TTable : Table
     where TTable2 : Table
    {
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        private readonly TTable2 _table2;
        public JoinBuilder(TTable table, TTable2 table2, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
            _table2 = table2;
        }

        public T On(params Func<TTable, TTable2, IBooleanPart>[] p) =>
            _apply(p.Select(f => f(_table, _table2)).ToArray());
    }

    public sealed class JoinBuilder<T, TTable, TTable2, TTable3>
     where TTable : Table
     where TTable2 : Table
     where TTable3 : Table
    {
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        private readonly TTable2 _table2;
        private readonly TTable3 _table3;
        public JoinBuilder(TTable table, TTable2 table2, TTable3 table3, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
            _table2 = table2;
            _table3 = table3;
        }

        public T On(params Func<TTable, TTable2, TTable3, IBooleanPart>[] p) =>
            _apply(p.Select(f => f(_table, _table2, _table3)).ToArray());
    }

    public sealed class JoinBuilder<T, TTable, TTable2, TTable3, TTable4>
      where TTable : Table
      where TTable2 : Table
      where TTable3 : Table
      where TTable4 : Table
    {
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        private readonly TTable2 _table2;
        private readonly TTable3 _table3;
        private readonly TTable4 _table4;
        public JoinBuilder(TTable table, TTable2 table2, TTable3 table3, TTable4 table4, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
            _table2 = table2;
            _table3 = table3;
            _table4 = table4;
        }

        public T On(params Func<TTable, TTable2, TTable3, TTable4, IBooleanPart>[] p) =>
            _apply(p.Select(f => f(_table, _table2, _table3, _table4)).ToArray());
    }

    public sealed class JoinBuilder<T, TTable, TTable2, TTable3, TTable4, TTable5>
      where TTable : Table
      where TTable2 : Table
      where TTable3 : Table
      where TTable4 : Table
      where TTable5 : Table
    {
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        private readonly TTable2 _table2;
        private readonly TTable3 _table3;
        private readonly TTable4 _table4;
        private readonly TTable5 _table5;
        public JoinBuilder(TTable table, TTable2 table2, TTable3 table3, TTable4 table4, TTable5 table5, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
            _table2 = table2;
            _table3 = table3;
            _table4 = table4;
            _table5 = table5;
        }

        public T On(params Func<TTable, TTable2, TTable3, TTable4, TTable5, IBooleanPart>[] p) =>
            _apply(p.Select(f => f(_table, _table2, _table3, _table4, _table5)).ToArray());
    }

    public sealed class JoinBuilder<T, TTable, TTable2, TTable3, TTable4, TTable5, TTable6>
       where TTable : Table
       where TTable2 : Table
       where TTable3 : Table
       where TTable4 : Table
       where TTable5 : Table
       where TTable6 : Table
    {
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        private readonly TTable2 _table2;
        private readonly TTable3 _table3;
        private readonly TTable4 _table4;
        private readonly TTable5 _table5;
        private readonly TTable6 _table6;
        public JoinBuilder(TTable table, TTable2 table2, TTable3 table3, TTable4 table4, TTable5 table5, TTable6 table6, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
            _table2 = table2;
            _table3 = table3;
            _table4 = table4;
            _table5 = table5;
            _table6 = table6;
        }

        public T On(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, IBooleanPart>[] p) =>
            _apply(p.Select(f => f(_table, _table2, _table3, _table4, _table5, _table6)).ToArray());
    }

    public sealed class JoinBuilder<T, TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7>
       where TTable : Table
       where TTable2 : Table
       where TTable3 : Table
       where TTable4 : Table
       where TTable5 : Table
       where TTable6 : Table
       where TTable7 : Table
    {
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        private readonly TTable2 _table2;
        private readonly TTable3 _table3;
        private readonly TTable4 _table4;
        private readonly TTable5 _table5;
        private readonly TTable6 _table6;
        private readonly TTable7 _table7;
        public JoinBuilder(TTable table, TTable2 table2, TTable3 table3, TTable4 table4, TTable5 table5, TTable6 table6, TTable7 table7, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
            _table2 = table2;
            _table3 = table3;
            _table4 = table4;
            _table5 = table5;
            _table6 = table6;
            _table7 = table7;
        }

        public T On(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, IBooleanPart>[] p) =>
            _apply(p.Select(f => f(_table, _table2, _table3, _table4, _table5, _table6, _table7)).ToArray());
    }

    public sealed class JoinBuilder<T, TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8>
        where TTable : Table
        where TTable2 : Table
        where TTable3 : Table
        where TTable4 : Table
        where TTable5 : Table
        where TTable6 : Table
        where TTable7 : Table
        where TTable8 : Table
    {
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        private readonly TTable2 _table2;
        private readonly TTable3 _table3;
        private readonly TTable4 _table4;
        private readonly TTable5 _table5;
        private readonly TTable6 _table6;
        private readonly TTable7 _table7;
        private readonly TTable8 _table8;
        public JoinBuilder(TTable table, TTable2 table2, TTable3 table3, TTable4 table4, TTable5 table5, TTable6 table6, TTable7 table7, TTable8 table8, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
            _table2 = table2;
            _table3 = table3;
            _table4 = table4;
            _table5 = table5;
            _table6 = table6;
            _table7 = table7;
            _table8 = table8;
        }

        public T On(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, IBooleanPart>[] p) =>
            _apply(p.Select(f => f(_table, _table2, _table3, _table4, _table5, _table6, _table7, _table8)).ToArray());
    }

    public sealed class JoinBuilder<T, TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9>
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
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        private readonly TTable2 _table2;
        private readonly TTable3 _table3;
        private readonly TTable4 _table4;
        private readonly TTable5 _table5;
        private readonly TTable6 _table6;
        private readonly TTable7 _table7;
        private readonly TTable8 _table8;
        private readonly TTable9 _table9;
        public JoinBuilder(TTable table, TTable2 table2, TTable3 table3, TTable4 table4, TTable5 table5, TTable6 table6, TTable7 table7, TTable8 table8, TTable9 table9, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
            _table2 = table2;
            _table3 = table3;
            _table4 = table4;
            _table5 = table5;
            _table6 = table6;
            _table7 = table7;
            _table8 = table8;
            _table9 = table9;
        }

        public T On(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, IBooleanPart>[] p) => 
            _apply(p.Select(f => f(_table, _table2, _table3, _table4, _table5, _table6, _table7, _table8, _table9)).ToArray());
    }

    public sealed class JoinBuilder<T, TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10> 
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
        private readonly Func<IBooleanPart[], T> _apply;
        private readonly TTable _table;
        private readonly TTable2 _table2;
        private readonly TTable3 _table3;
        private readonly TTable4 _table4;
        private readonly TTable5 _table5;
        private readonly TTable6 _table6;
        private readonly TTable7 _table7;
        private readonly TTable8 _table8;
        private readonly TTable9 _table9;
        private readonly TTable10 _table10;
        public JoinBuilder(TTable table, TTable2 table2, TTable3 table3, TTable4 table4, TTable5 table5, TTable6 table6, TTable7 table7, TTable8 table8, TTable9 table9, TTable10 table10, Func<IBooleanPart[], T> f)
        {
            _apply = f;
            _table = table;
            _table2 = table2;
            _table3 = table3;
            _table4 = table4;
            _table5 = table5;
            _table6 = table6;
            _table7 = table7;
            _table8 = table8;
            _table9 = table9;
            _table10 = table10;
        }

        public T On(params Func<TTable, TTable2, TTable3, TTable4, TTable5, TTable6, TTable7, TTable8, TTable9, TTable10, IBooleanPart>[] p)
            => _apply(p.Select(f => f(_table, _table2, _table3, _table4, _table5, _table6, _table7, _table8, _table9, _table10)).ToArray());
    }

    

    public sealed class InnerJoinPart : JoinPart
    {
        internal IBooleanPart[] Conditions;
        internal IFromPart From;

        public InnerJoinPart(IFromPart from, params IBooleanPart[] conditions)
        {
            From = @from;
            Conditions = conditions;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class LeftJoinPart : JoinPart
    {
        internal IBooleanPart[] Conditions;
        internal IFromPart From;

        public LeftJoinPart(IFromPart from, params IBooleanPart[] conditions)
        {
            From = @from;
            Conditions = conditions;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class RightJoinPart : JoinPart
    {
        internal IBooleanPart[] Conditions;
        internal IFromPart From;

        public RightJoinPart(IFromPart from, params IBooleanPart[] conditions)
        {
            From = @from;
            Conditions = conditions;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
}