using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder.Visitors
{
    public interface IVisitorResult
    {
        void Start();

        IVisitorResult Append(string? str);
        void AddParameter<T>(IParameter<T> para);

        void Finished();
        IVisitorResult Append(int value);
    }
}