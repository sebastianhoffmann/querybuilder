using System;
using System.Data.Common;
using System.Linq;
using Deviax.QueryBuilder.Visitors;

namespace Deviax.QueryBuilder
{
    public class PostgresExecutor : QueryExecutor
    {
        public class DefaultPostgresNameResolver : INameResolver
        {
            public string DbToCSharp(string name)
            {
                if (name.StartsWith("n_"))
                    name = name.Substring(2);

                return string.Join("", name.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Substring(0, 1).ToUpper() + s.Substring(1)).ToArray());
            }
            public string CSharpToDb(string csharpName, bool nullable) => string.Concat(nullable ? "n_" : "", string.Join(string.Empty, csharpName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()))).ToLower();

        }

        public PostgresExecutor() : this(new DefaultPostgresNameResolver()) { }
        public PostgresExecutor(INameResolver resolver)
        {
            DefaultNameResolver = resolver;
        }
        protected override INameResolver DefaultNameResolver { get; }
        
    }
}