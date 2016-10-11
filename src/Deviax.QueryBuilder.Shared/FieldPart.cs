using System;
using System.Linq;
using Deviax.QueryBuilder.Parts;
using Deviax.QueryBuilder.Visitors;
using System.Collections.Generic;
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
        public EqPart Eq<T>(T value) => new EqPart(this, new Parameter<T>(value, Name));

        [Pure]
        public SetFieldPart Set<T>(T value) => new SetFieldPart(this, new Parameter<T>(value, Name));
        [Pure]
        public SetPart SetWithPart(IPart part) => new SetPart(this, part);
        [Pure]
        public NeqPart Neq<T>(T value) => new NeqPart(this, new Parameter<T>(value, Name));
       
        [Pure]
        public GtPart Gt<T>(T value) => new GtPart(this, new Parameter<T>(value, Name));
        
        [Pure]
        public GtePart Gte<T>(T value) => new GtePart(this, new Parameter<T>(value, Name));
       
        [Pure]
        public LtPart Lt<T>(T value) => new LtPart(this, new Parameter<T>(value, Name));
        
        [Pure]
        public LtePart Lte<T>(T value) => new LtePart(this, new Parameter<T>(value, Name));

        [Pure]
        public BetweenPart Between<T>(T left, T right) => new BetweenPart(this, new Parameter<T>(left, Name + "_l"), new Parameter<T>(right, Name + "_r"));

       [Pure]
        public InPart In<T>(T item, T item2, params T[] items) => new InPart(this, new ArrayParameter<T>(new [] { item, item2 }.Concat(items), Name));

        [Pure]
        public InPart In<T>(IEnumerable<T> items) => new InPart(this, new ArrayParameter<T>(items, Name));

        [Pure]public static PlusPart operator +(Field left, int right) => new PlusPart(left, new Parameter<int>(right, left.Name));
        [Pure]public static PlusPart operator +(Field left, long right) => new PlusPart(left, new Parameter<long>(right, left.Name));
        [Pure]public static PlusPart operator +(Field left, float right) => new PlusPart(left, new Parameter<float>(right, left.Name));
        [Pure]public static PlusPart operator +(Field left, double right) => new PlusPart(left, new Parameter<double>(right, left.Name));
        [Pure]public static PlusPart operator +(Field left, decimal right) => new PlusPart(left, new Parameter<decimal>(right, left.Name));
        [Pure]public static PlusPart operator +(Field left, DateTime right) => new PlusPart(left, new Parameter<DateTime>(right, left.Name));
       
        [Pure]
        public static MinusPart operator -(Field left, int right) => new MinusPart(left, new Parameter<int>(right, left.Name));
        [Pure]
        public static MinusPart operator -(Field left, long right) => new MinusPart(left, new Parameter<long>(right, left.Name));
        [Pure]
        public static MinusPart operator -(Field left, float right) => new MinusPart(left, new Parameter<float>(right, left.Name));
        [Pure]
        public static MinusPart operator -(Field left, double right) => new MinusPart(left, new Parameter<double>(right, left.Name));
        [Pure]
        public static MinusPart operator -(Field left, decimal right) => new MinusPart(left, new Parameter<decimal>(right, left.Name));

        [Pure]
        public static MulPart operator *(Field left, int right) => new MulPart(left, new Parameter<int>(right, left.Name));
        [Pure]
        public static MulPart operator *(Field left, long right) => new MulPart(left, new Parameter<long>(right, left.Name));
        [Pure]
        public static MulPart operator *(Field left, float right) => new MulPart(left, new Parameter<float>(right, left.Name));
        [Pure]
        public static MulPart operator *(Field left, double right) => new MulPart(left, new Parameter<double>(right, left.Name));
        [Pure]
        public static MulPart operator *(Field left, decimal right) => new MulPart(left, new Parameter<decimal>(right, left.Name));
        

        [Pure]
        public static DivPart operator /(Field left, int right) => new DivPart(left, new Parameter<int>(right, left.Name));
        [Pure]
        public static DivPart operator /(Field left, long right) => new DivPart(left, new Parameter<long>(right, left.Name));
        [Pure]
        public static DivPart operator /(Field left, float right) => new DivPart(left, new Parameter<float>(right, left.Name));
        [Pure]
        public static DivPart operator /(Field left, double right) => new DivPart(left, new Parameter<double>(right, left.Name));
        [Pure]
        public static DivPart operator /(Field left, decimal right) => new DivPart(left, new Parameter<decimal>(right, left.Name));


        [Pure]
        public static ModPart operator %(Field left, int right) => new ModPart(left, new Parameter<int>(right, left.Name));
        [Pure]
        public static ModPart operator %(Field left, long right) => new ModPart(left, new Parameter<long>(right, left.Name));
        [Pure]
        public static ModPart operator %(Field left, float right) => new ModPart(left, new Parameter<float>(right, left.Name));
        [Pure]
        public static ModPart operator %(Field left, double right) => new ModPart(left, new Parameter<double>(right, left.Name));
        [Pure]
        public static ModPart operator %(Field left, decimal right) => new ModPart(left, new Parameter<decimal>(right, left.Name));
    }
}