using System;

namespace Borg.Infrastructure.Core.Reflection.Discovery.ObjectGraph
{
    public struct ComplexTypeRecursorResultItem
    {
        internal ComplexTypeRecursorResultItem(Type type, Type referrer = null, int recursion = 0)
        {
            Type = Preconditions.NotNull(type, nameof(type));
            Referrer = referrer;
            Recursion = Preconditions.PositiveOrZero(recursion, nameof(recursion));
        }

        public Type Type { get; }
        public Type Referrer { get; }
        public int Recursion { get; }
    }
}