using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder
{
    public sealed partial class Field
    {
        public LikePart Like(string value) => new LikePart(this, new Parameter<string>(value, Name), LikeMode.CaseSensitive);
        // ReSharper disable once InconsistentNaming
        public LikePart ILike(string value) => new LikePart(this, new Parameter<string>(value, Name), LikeMode.IgnoreCase);
    }
}