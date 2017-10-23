using System;
using System.Linq;

namespace Deviax.QueryBuilder
{
    public class MySqlExecutor : QueryExecutor
    {
        public MySqlExecutor() : this(new DefaultMySqlNameResolver()) {}

        public MySqlExecutor(INameResolver resolver)
        {
            DefaultNameResolver = resolver;
        }

        protected override INameResolver DefaultNameResolver { get; }

        public class DefaultMySqlNameResolver : INameResolver
        {
            public unsafe string DbToCSharp(string name)
            {
                return name;
            }

            public unsafe string CSharpToDb(string csharpName)
            {
                return csharpName;
            }
        }
    }
}