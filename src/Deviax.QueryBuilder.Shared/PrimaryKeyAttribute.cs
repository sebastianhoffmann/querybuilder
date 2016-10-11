using System;

namespace Deviax.QueryBuilder
{
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(params string[] fields)
        {
            Fields = fields;
        }

        public string[] Fields;
    }
}
