namespace Deviax.QueryBuilder.Visitors
{
    public interface IQueryingVisitor<T>
    {
        void Process(T q);
    }
}