namespace Deviax.QueryBuilder.Visitors
{
    public interface IVisitorResult
    {
        void Start();

        IVisitorResult Append(string str);
        void AddParameter<T>(string name, T value);

        void Finished();
        IVisitorResult Append(int value);
    }
}