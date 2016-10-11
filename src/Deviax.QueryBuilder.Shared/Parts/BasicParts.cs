using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder.Parts
{
    public abstract partial class Part : IPart
    {
        public abstract void Accept(INodeVisitor visitor);

        [Pure]public SetPart Set(IPart value) => new SetPart(this, value);
        [Pure]public EqPart Eq(IPart other) => new EqPart(this, other);
        [Pure]public NeqPart Neq(IPart other) => new NeqPart(this, other);
        [Pure]public IsNotNullPart IsNotNull() => new IsNotNullPart(this);
        [Pure]public IsNullPart IsNull() => new IsNullPart(this);
        [Pure]public IsFalsePart IsFalse() => new IsFalsePart(this);
        [Pure]public IsTruePart IsTrue() => new IsTruePart(this);
        [Pure]public BetweenPart Between(IPart left, IPart right) => new BetweenPart(this, left, right);
       
        [Pure]public LtPart Lt(IPart other) => new LtPart(this, other);
        [Pure]public LtePart Lte(IPart other) => new LtePart(this, other);
        [Pure]public GtPart Gt(IPart other) => new GtPart(this, other);
        [Pure]public GtePart Gte(IPart other) => new GtePart(this, other);
        [Pure]public InPart In(IPart part) => new InPart(this, part);
        [Pure]public InPart In<T>(IEnumerable<T> items, string parameterName) => new InPart(this, new ArrayParameter<T>(items, parameterName));

        [Pure]
        public AscOrdering Asc(Nulls n = Nulls.Unspecified) => new AscOrdering(this, n);
        [Pure]
        public DescOrdering Desc(Nulls n = Nulls.Unspecified) => new DescOrdering(this, n);

        [Pure]
        public AndPart And(IPart other) => new AndPart(this, other);
        [Pure]
        public OrPart Or(IPart other) => new OrPart(this, other);

        [Pure]public AliasPart As(string alias) => new AliasPart(alias, this);

        [Pure]public static PlusPart operator +(Part left, IPart right) => new PlusPart(left, right);
        [Pure]public static MinusPart operator -(Part left, IPart right) => new MinusPart(left, right);
        [Pure]public static MulPart operator *(Part left, IPart right) => new MulPart(left, right);
        [Pure]public static DivPart operator /(Part left, IPart right) => new DivPart(left, right);
        [Pure]public static ModPart operator %(Part left, IPart right) => new ModPart(left, right);
    }

    public interface ISetPart : IPart { }

    public class SetFieldPart : ISetPart
    {
        public readonly Field Field;
        public readonly IPart Value;

        public SetFieldPart(Field field, IPart value)
        {
            Field = field;
            Value = value;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class SetPart : ISetPart
    {
        public readonly IPart Left;
        public readonly IPart Value;

        public SetPart(IPart left, IPart value)
        {
            Left = left;
            Value = value;
        }

        public void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public abstract class LeftRightPart : Part
    {
        public readonly IPart Left;
        public readonly IPart Right;

        protected LeftRightPart(IPart left, IPart right)
        {

            Left = left;
            Right = right;
        }
    }

    public class AliasPart : Part, IAliasPart, IBooleanPart {
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        public AliasPart(string name, IPart part)
        {
            Name = name;
            Aliased = part;
        }

        public string Name { get; }
        public IPart Aliased { get; }
    }

    public partial class RawSql : Part, IBooleanPart
    {
        internal readonly string Sql;
        public RawSql(string sql)
        {
            Sql = sql;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }

    public class Literal<T> : Part
    {
        public readonly T Value;
        public Literal(T v)
        {
            Value = v;
        }

        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);
    }
    
    public interface IPart
    {
        void Accept(INodeVisitor visitor);
    }
    
    public interface INamedPart { string Name { get; } }

    public interface IField : INamedPart
    {
        Table Table { get; }
    }

    public interface IAliasPart : INamedPart, IPart
    {
        IPart Aliased { get; }
    }

    public interface IFromPart
    {
        void Accept(INodeVisitor visitor);
    }
}

