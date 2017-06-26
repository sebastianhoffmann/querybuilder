using System;
using System.Collections.Generic;
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
            var tableArg = Expression.Parameter(typeof(Table));
            var objArg = Expression.Parameter(typeof(object));
            if (pk != null)
            {
               
                var baseEqMethod = typeof(Field).GetMethods().First(m => m.Name == "EqV" && m.IsGenericMethod);

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
                                baseEqMethod.MakeGenericMethod(prop.PropertyType), Expression.Property(Expression.Convert(objArg, typeof(T)), prop), Expression.Constant(null, typeof(string)));
                        }
                        return Expression.Call(Expression.Field(Expression.Convert(tableArg, typeof(TTable)), fi), baseEqMethod.MakeGenericMethod(originalField.FieldType), Expression.Field(Expression.Convert(objArg, typeof(T)), originalField), Expression.Constant(null, typeof(string)));
                    })
                ), objArg, tableArg);
                TypeToTableEntry<T>.GetDefaultConditions = l.Compile();
            }

            var vcVar = Expression.Variable(typeof(ValuesCollection));
            var addMethod = typeof(ValuesCollection).GetMethod("Add");
            var setVMethod = typeof(Field).GetMethod(nameof(Field.SetV));
            var tableFields = typeof(TTable).GetFields().Where(f => f.Name != "TableName" && f.Name != "TableSchema" && f.Name != "TableAlias").ToList();
            var tableProps = typeof(TTable).GetProperties();
            var nArg = Expression.Parameter(typeof(int));
            var concatMethod = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });
            var intToStr = typeof(int).GetMethod("ToString", Type.EmptyTypes);

            var memberThings = tableFields.Select(f => {
                MemberExpression memberExpression = null;
                Type targetType;
                MethodCallExpression setMethod;
                MemberExpression tableFieldExpr = Expression.Field(Expression.Convert(tableArg, typeof(TTable)), f); 

                var paraNameExpr = Expression.Call(null, concatMethod, Expression.Constant(f.Name), Expression.Call(nArg, intToStr));

                var originalField = typeof(T).GetField(f.Name);
                if (originalField == null)
                {
                    var prop = typeof(T).GetProperty(f.Name);

                    if (prop == null)
                        throw new ArgumentException($"Type `{typeof(T).FullName}` does not have a member of name `{f.Name}`");

                    memberExpression = Expression.Property(Expression.Convert(objArg, typeof(T)), prop);
                    targetType = prop.PropertyType;

                    setMethod = Expression.Call(tableFieldExpr,setVMethod.MakeGenericMethod(prop.PropertyType), Expression.Property(Expression.Convert(objArg, typeof(T)), prop), paraNameExpr);
                }
                else
                {
                    memberExpression = Expression.Field(Expression.Convert(objArg, typeof(T)), originalField);
                     
                    targetType = originalField.FieldType;
                    setMethod = Expression.Call(tableFieldExpr,setVMethod.MakeGenericMethod(originalField.FieldType), Expression.Field(Expression.Convert(objArg, typeof(T)), originalField), paraNameExpr);
                }

                return Tuple.Create(memberExpression, targetType, f.Name, setMethod, tableFieldExpr);
            }).Concat(tableProps.Select(
                p => {
                    MemberExpression memberExpression = null;
                    Type targetType;
                    MethodCallExpression setMethod;
                    MemberExpression tableFieldExpr = Expression.Property(Expression.Convert(tableArg, typeof(TTable)), p); ;

                    var paraNameExpr = Expression.Call(null, concatMethod, Expression.Constant(p.Name), Expression.Call(null, intToStr, nArg));
                    var originalField = typeof(T).GetField(p.Name);
                    if (originalField == null)
                    {
                        var prop = typeof(T).GetProperty(p.Name);

                        if (prop == null)
                            throw new ArgumentException($"Type `{typeof(T).FullName}` does not have a member of name `{p.Name}`");

                        memberExpression = Expression.Property(Expression.Convert(objArg, typeof(T)), prop);
                        targetType = prop.PropertyType;
                        setMethod = Expression.Call(Expression.Property(Expression.Convert(tableArg, typeof(TTable)), p),
                            setVMethod.MakeGenericMethod(prop.PropertyType), Expression.Property(Expression.Convert(objArg, typeof(T)), prop), paraNameExpr);
                    }
                    else
                    {
                        memberExpression = Expression.Field(Expression.Convert(objArg, typeof(T)), originalField);
                        targetType = originalField.FieldType;
                        setMethod = Expression.Call(tableFieldExpr,setVMethod.MakeGenericMethod(originalField.FieldType), Expression.Field(Expression.Convert(objArg, typeof(T)), originalField), paraNameExpr);
                    }

                    return Tuple.Create(memberExpression, targetType, p.Name, setMethod, tableFieldExpr);
                })).ToList();

            var blockExpressions = new List<Expression> { Expression.Assign(vcVar, Expression.New(typeof(ValuesCollection))) };
            
            foreach (var thing in memberThings)
            {
                var setExpr = Expression.Assign(vcVar, Expression.Call(vcVar, addMethod,
                    Expression.NewArrayInit(typeof(SetFieldPart), thing.Item4)
                ));

                if (pk != null && pk.Fields.Any(f => f == thing.Item3))
                {
                    blockExpressions.Add(
                        Expression.IfThen(Expression.Not(Expression.Equal(Expression.Default(thing.Item2), thing.Item1)), setExpr)
                    );
                }
                else
                {
                    blockExpressions.Add(setExpr);
                }
            }

            blockExpressions.Add(vcVar);

            TypeToTableEntry<T>.ToValues = Expression.Lambda<Func<object, Table, int, ValuesCollection>>(
                Expression.Block(
                    new[] {
                            vcVar
                    },
                    blockExpressions.ToArray()),
                objArg,
                tableArg,
                nArg).Compile();


            if (pk != null && pk.Fields.Length == 1)
            {
                var mt = memberThings.Single(x => x.Item3 == pk.Fields[0]);
                if (mt.Item2 == typeof(int) || mt.Item2 == typeof(long))
                {
                    var partArr = Expression.Variable(typeof(IPart[]));
                    TypeToTableEntry<T>.Returning = Expression.Lambda<Func<object, Table, IPart[]>>(
                        Expression.Block(
                            new [] { partArr },
                        Expression.IfThen(Expression.Equal(Expression.Default(mt.Item2), mt.Item1), 
                            Expression.Assign(partArr, Expression.NewArrayInit(typeof(IPart), mt.Item5))),
                        partArr),
                        new [] { objArg, tableArg}
                    ).Compile();

                    var obj2Arg = Expression.Parameter(typeof(object));

                    if (mt.Item2 == typeof(long))
                    {
                        var conversionMethod = typeof(Convert).GetMethod(
                            "ToInt64",
                            new Type[] {
                                typeof(object)
                            });
                        
                        TypeToTableEntry<T>.ApplyReturning = Expression.Lambda<Action<object, object>>(
                            Expression.Assign(mt.Item1, Expression.Call(conversionMethod, obj2Arg)) ,
                            new[] {
                                objArg,
                                obj2Arg
                            }
                        ).Compile();
                    }
                    else
                    {
                        TypeToTableEntry<T>.ApplyReturning = Expression.Lambda<Action<object, object>>(
                            Expression.Assign(mt.Item1, Expression.Convert(obj2Arg, mt.Item2)),
                            new[] {
                                objArg,
                                obj2Arg
                            }
                        ).Compile();
                    }
                   
                }
            }
        }
    }

    internal static class TypeToTableEntry<T>
    {
        public static Func<string, Table> TableConstructor;
        public static Func<object, Table, IBooleanPart[]> GetDefaultConditions;
        public static Func<object, Table, int, ValuesCollection> ToValues;
        public static Func<object, Table, IPart[]> Returning = (o,t) => null;
        public static Action<object, object> ApplyReturning = (o,t) => {};
        public static Table DefaultTable;
    }
}
