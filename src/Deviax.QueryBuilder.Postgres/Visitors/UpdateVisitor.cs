using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public partial class UpdateVisitor
    {
        public override void Visit(SetFieldPart sfp)
        {
            sfp.Field.Accept(this);
            Result.Append("= ");
            sfp.Value.Accept(this);
        }

        public override void Visit(SetPart sp)
        {
            sp.Left.Accept(this);
            Result.Append("= ");
            sp.Value.Accept(this);
        }
    }
}