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
            public string DbToCSharp(string name)
            {
                if (name.StartsWith("n_"))
                    name = name.Substring(2);

                return string.Join("", name.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Substring(0, 1).ToUpper() + s.Substring(1)).ToArray());
            }

            public string CSharpToDb(string csharpName)
                => string.Join(string.Empty, csharpName.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}