using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Deviax.QueryBuilder.Parts;
using NpgsqlTypes;

namespace Deviax.QueryBuilder
{
    public sealed partial class Field
    {
        public NpgsqlDbType? NpgsqlDbType { get; internal set; }

        private Parameter<T> CreateParameter<T>(T value, string name) =>
            NpgsqlDbType.HasValue
                ? new Parameter<T>(value, name, NpgsqlDbType.Value)
                : new Parameter<T>(value, name);

        private ArrayParameter<T> CreateArrayParameter<T>(IEnumerable<T> values, string name) =>
            NpgsqlDbType.HasValue
                ? new ArrayParameter<T>(
                    values,
                    name,
                    NpgsqlTypes.NpgsqlDbType.Array | NpgsqlDbType.Value
                )
                : new ArrayParameter<T>(values, name);

        [Pure]
        public LikePart Like(string value, string? name = null) =>
            new LikePart(this, CreateParameter(value, name ?? Name), LikeMode.CaseSensitive);

        [Pure] // ReSharper disable once InconsistentNaming
        public LikePart ILike(string value, string? name = null) =>
            new LikePart(this, CreateParameter(value, name ?? Name), LikeMode.IgnoreCase);

        [Pure]
        public ContainsPart Contains(IPart other) => new ContainsPart(this, other);

        [Pure]
        public ContainsPart ContainsV<T>(T val, string? name = null) =>
            new ContainsPart(this, CreateParameter(val, name ?? Name));

        [Pure]
        public MatchesRegexPart MatchesRegex(IPart part) => new MatchesRegexPart(this, part);

        [Pure]
        public MatchesRegexPart MatchesRegex(string regex, string? name = null) =>
            new MatchesRegexPart(this, CreateParameter(regex, name ?? Name));
    }
}
