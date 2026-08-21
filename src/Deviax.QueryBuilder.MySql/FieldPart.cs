using System.Collections.Generic;
using Deviax.QueryBuilder.Parts;

namespace Deviax.QueryBuilder
{
    public sealed partial class Field
    {
        private Parameter<T> CreateParameter<T>(T value, string name) =>
            new Parameter<T>(value, name);

        private ArrayParameter<T> CreateArrayParameter<T>(IEnumerable<T> values, string name) =>
            new ArrayParameter<T>(values, name);
    }
}
