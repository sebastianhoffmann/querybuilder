using System;
using System.Collections.Generic;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;
using System.Diagnostics.Contracts;

namespace Deviax.QueryBuilder
{
    public sealed partial class Field : Part, IField
    {
        public Field(Table table, string name)
        {
            Table = table;
            Name = name;
        }

        public string Name { get; }

        public Table Table { get; }
        public override void Accept(INodeVisitor visitor) => visitor.Visit(this);

        [Pure]
        public SetFieldPart Set(IPart part) => new SetFieldPart(this, part);

        [Pure]
        public EqPart EqV<T>(T value, string? name = null) =>
            new EqPart(this, CreateParameter(value, name ?? Name));

        [Pure]
        public SetFieldPart SetV<T>(T value, string? name = null) =>
            new SetFieldPart(this, CreateParameter(value, name ?? Name));

        [Pure]
        public NeqPart NeqV<T>(T value, string? name = null) =>
            new NeqPart(this, CreateParameter(value, name ?? Name));

        [Pure]
        public GtPart GtV<T>(T value, string? name = null) =>
            new GtPart(this, CreateParameter(value, name ?? Name));

        [Pure]
        public GtePart GteV<T>(T value, string? name = null) =>
            new GtePart(this, CreateParameter(value, name ?? Name));

        [Pure]
        public LtPart LtV<T>(T value, string? name = null) =>
            new LtPart(this, CreateParameter(value, name ?? Name));

        [Pure]
        public LtePart LteV<T>(T value, string? name = null) =>
            new LtePart(this, CreateParameter(value, name ?? Name));

        [Pure]
        public BetweenPart BetweenV<T>(
            T left,
            T right,
            string? leftName = null,
            string? rightName = null
        ) =>
            new BetweenPart(
                this,
                CreateParameter(left, leftName ?? Name + "_l"),
                CreateParameter(right, rightName ?? Name + "_r")
            );

        [Pure]
        public InPart InV<T>(IEnumerable<T> items, string? name = null) =>
            new InPart(this, CreateArrayParameter(items, name ?? Name));

        [Pure]
        public static PlusPart operator +(Field left, int right) =>
            new PlusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static PlusPart operator +(Field left, long right) =>
            new PlusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static PlusPart operator +(Field left, float right) =>
            new PlusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static PlusPart operator +(Field left, double right) =>
            new PlusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static PlusPart operator +(Field left, decimal right) =>
            new PlusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static PlusPart operator +(Field left, DateTime right) =>
            new PlusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MinusPart operator -(Field left, int right) =>
            new MinusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MinusPart operator -(Field left, long right) =>
            new MinusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MinusPart operator -(Field left, float right) =>
            new MinusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MinusPart operator -(Field left, double right) =>
            new MinusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MinusPart operator -(Field left, decimal right) =>
            new MinusPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MulPart operator *(Field left, int right) =>
            new MulPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MulPart operator *(Field left, long right) =>
            new MulPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MulPart operator *(Field left, float right) =>
            new MulPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MulPart operator *(Field left, double right) =>
            new MulPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static MulPart operator *(Field left, decimal right) =>
            new MulPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static DivPart operator /(Field left, int right) =>
            new DivPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static DivPart operator /(Field left, long right) =>
            new DivPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static DivPart operator /(Field left, float right) =>
            new DivPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static DivPart operator /(Field left, double right) =>
            new DivPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static DivPart operator /(Field left, decimal right) =>
            new DivPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static ModPart operator %(Field left, int right) =>
            new ModPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static ModPart operator %(Field left, long right) =>
            new ModPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static ModPart operator %(Field left, float right) =>
            new ModPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static ModPart operator %(Field left, double right) =>
            new ModPart(left, left.CreateParameter(right, left.Name));

        [Pure]
        public static ModPart operator %(Field left, decimal right) =>
            new ModPart(left, left.CreateParameter(right, left.Name));
    }
}
