using System;
using System.Linq;

namespace Deviax.QueryBuilder
{
    public class PostgresExecutor : QueryExecutor
    {
        public PostgresExecutor() : this(new DefaultPostgresNameResolver()) {}

        public PostgresExecutor(INameResolver resolver)
        {
            DefaultNameResolver = resolver;
        }

        protected override INameResolver DefaultNameResolver { get; }

        public class DefaultPostgresNameResolver : INameResolver
        {
            public unsafe string DbToCSharp(string name)
            {
                var len = name.Length;
                var chars = stackalloc char[len];
                
                var writer = 0;
                var nextUpper = true;

                for (int i = 0; i < len; i++)
                {
                    if (name[i] == '_')
                    {
                        nextUpper = true;
                    }
                    else if (nextUpper)
                    {
                        chars[writer++] = char.ToUpper(name[i]);
                        nextUpper = false;
                    }
                    else
                    {
                        chars[writer++] = name[i];
                    }
                }

                return new string(chars, 0, writer);
            }

            public unsafe string CSharpToDb(string csharpName)
            {
                var len = csharpName.Length;
                var chars = stackalloc char[len * 2];
                var writer = 0;

                for (int i = 0; i < len; i++)
                {
                    if (char.IsUpper(csharpName[i]))
                    {
                        if(writer > 0)
                            chars[writer++] = '_';

                        chars[writer++] = char.ToLower(csharpName[i]);
                    }
                    else
                    {
                        chars[writer++] = csharpName[i];
                    }
                }
                return new string(chars, 0, writer);
            }
        }
    }
}