using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;

namespace Deviax.QueryBuilder
{
    internal abstract class TypeConversionSpecialization
    {
        public abstract bool Matches(TypeInfo ti, Type t);
        public abstract Expression Convert(TypeInfo ti, Type t, ParameterExpression valueVariable);
    }
    
    internal class ListTypeConversionSpecialization : TypeConversionSpecialization
    {
        private static readonly Type GenericListType = typeof(List<>);
        public override bool Matches(TypeInfo ti, Type t) => ti.IsGenericType && t.GetGenericTypeDefinition() == GenericListType;

        public override Expression Convert(TypeInfo ti, Type t, ParameterExpression valueVariable)
        {
            var genArg = ti.GetGenericArguments().Single();
            var iEnumerableConstructor = ti.GetConstructors().Single(
                c => c.GetParameters().Any(p => p.ParameterType.GetTypeInfo().IsGenericType));
            return Expression.New(iEnumerableConstructor, Expression.Convert(valueVariable, genArg.MakeArrayType()));
        }
    }
    
    internal class DefaultTypeConversionSpecialization : TypeConversionSpecialization
    {
        public override bool Matches(TypeInfo ti, Type t) => true;

        public override Expression Convert(TypeInfo ti, Type t, ParameterExpression valueVariable)
        {
            return Expression.Convert(valueVariable, t);
        }
    }
}