using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder
{
    public sealed partial class Field
    {
        [Pure]
        public LikePart Like(string value) => new LikePart(this, new Parameter<string>(value, Name), LikeMode.CaseSensitive);

        [Pure] // ReSharper disable once InconsistentNaming
        public LikePart ILike(string value) => new LikePart(this, new Parameter<string>(value, Name), LikeMode.IgnoreCase);

        [Pure]
        public ContainsPart Contains(IPart other) => new ContainsPart(this, other);

        [Pure]
        public ContainsPart ContainsV<T>(T val) => new ContainsPart(this, new Parameter<T>(val, Name));

        [Pure]
        public MatchesRegexPart MatchesRegex(IPart part) => new MatchesRegexPart(this, part);

        [Pure]
        public MatchesRegexPart MatchesRegex(string regex) => new MatchesRegexPart(this, new Parameter<string>(regex, Name));
    }
}