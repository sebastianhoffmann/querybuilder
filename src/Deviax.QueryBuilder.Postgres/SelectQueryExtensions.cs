using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder
{
    public enum ForUpdateWaitPolicy
    {
        Wait,
        NoWait,
        SkipLocked,
    }

    public static class SelectQueryExtensions
    {
        [Pure]
        public static TQuery ForUpdate<TQuery>(
            this BaseSelectQuery<TQuery> query,
            ForUpdateWaitPolicy waitPolicy = ForUpdateWaitPolicy.Wait
        )
            where TQuery : BaseSelectQuery<TQuery>
        {
            return query.WithExtraParameter(new ForUpdatePart(waitPolicy));
        }
    }
}
