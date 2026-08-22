#if DEBUG
using System;
using System.Collections;

namespace Deviax.QueryBuilder.Visitors
{
    internal static class ParameterValidator
    {
        public static void ValidateValue(string name, object? previous, object? value)
        {
            if (ValuesEqual(previous, value))
            {
                return;
            }

            throw new InvalidOperationException(
                $"Query parameter name '{name}' is used with different values. "
                    + "Use distinct parameter names for values that differ."
            );
        }

        private static bool ValuesEqual(object? left, object? right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is IStructuralEquatable structural)
            {
                return structural.Equals(right, StructuralComparisons.StructuralEqualityComparer);
            }

            return Equals(left, right);
        }
    }
}
#endif
