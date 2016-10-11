using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;
using Deviax.QueryBuilder.Parts;
namespace Deviax.QueryBuilder
{
    public class Registry
    {
        public static void RegisterTypeToTable<T, TTable>() where TTable : Table
        {
            var c = typeof(TTable).GetConstructor(
                new[] {
                    typeof(string)
                });

            if (c == null)
            {
                throw new ArgumentException($"Table type `{typeof(TTable).FullName}` does not have a constructor that takes a string (the alias)");
            }

            var a = Expression.Parameter(typeof(string));
            TypeToTableEntry<T>.TableConstructor = Expression.Lambda<Func<string, Table>>(Expression.Convert(Expression.New(c, a), typeof(Table)), a).Compile();
            TypeToTableEntry<T>.DefaultTable = TypeToTableEntry<T>.TableConstructor("");

            var pk = typeof(TTable).GetTypeInfo().GetCustomAttribute<PrimaryKeyAttribute>();

            if (pk != null)
            {
                var tableArg = Expression.Parameter(typeof(Table));
                var objArg = Expression.Parameter(typeof(object));
                var baseEqMethod = typeof(Field).GetMethods().First(m => m.Name == "Eq" && m.IsGenericMethod);

                var l = Expression.Lambda<Func<object, Table, IBooleanPart[]>>(
                Expression.NewArrayInit(typeof(IBooleanPart),
                    pk.Fields.Select(f => typeof(TTable).GetField(f)).Select(fi => {
                        var originalField = typeof(T).GetField(fi.Name);

                        if (originalField == null)
                        {
                            var prop = typeof(T).GetProperty(fi.Name);

                            if (prop == null)
                                throw new ArgumentException($"Type `{typeof(T).FullName}` does not have a member of name `{fi.Name}`");

                            return Expression.Call(Expression.Field(Expression.Convert(tableArg, typeof(TTable)), fi),
                                baseEqMethod.MakeGenericMethod(prop.PropertyType), Expression.Property(Expression.Convert(objArg, typeof(T)), prop));
                        }
                        return Expression.Call(Expression.Field(Expression.Convert(tableArg, typeof(TTable)), fi), baseEqMethod.MakeGenericMethod(originalField.FieldType), Expression.Field(Expression.Convert(objArg, typeof(T)), originalField));
                    })
                ), objArg, tableArg);
                TypeToTableEntry<T>.GetDefaultConditions = l.Compile();
            }
        }
    }

    internal static class TypeToTableEntry<T>
    {
        public static Func<string, Table> TableConstructor;
        public static Func<object, Table, IBooleanPart[]> GetDefaultConditions;
        public static Table DefaultTable;
    }
}
