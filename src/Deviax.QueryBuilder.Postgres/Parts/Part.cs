using System.Diagnostics.Contracts;

namespace Deviax.QueryBuilder.Parts
{
    public abstract partial class Part : IPart
    {
        [Pure]
        public LikePart Like(IPart other) => new LikePart(this, other, LikeMode.CaseSensitive);

        [Pure] // ReSharper disable once InconsistentNaming
        public LikePart ILike(IPart other) => new LikePart(this, other, LikeMode.IgnoreCase);
    }
}