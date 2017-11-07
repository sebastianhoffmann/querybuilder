using System;

namespace Deviax.QueryBuilder
{
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(bool singleKeyIsAutoincrement, params string[] fields)
        {
            Fields = fields;
            SingleKeyIsAutoincrement = singleKeyIsAutoincrement;
        }
        
        public PrimaryKeyAttribute(params string[] fields)
        {
            Fields = fields;
        }
        
        public string[] Fields;
        public bool SingleKeyIsAutoincrement = true;
    }
}
