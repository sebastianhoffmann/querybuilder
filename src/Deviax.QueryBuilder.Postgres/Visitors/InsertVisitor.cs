using System;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class InsertVisitor 
    {
        public override void Visit(SetFieldPart sfp)
        {
            if (State == CoarseState.ValuesDeclaration)
            {
                NoTableName = true;
                sfp.Field.Accept(this);
                NoTableName = false;
            }
            else if (State == CoarseState.Values)
            {
                sfp.Value.Accept(this);
            }
            else
            {
                throw new InvalidOperationException();
            }
            
        }
    }
}