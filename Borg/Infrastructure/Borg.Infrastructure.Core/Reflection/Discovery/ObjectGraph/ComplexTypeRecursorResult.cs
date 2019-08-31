using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infrastructure.Core.Reflection.Discovery.ObjectGraph
{
    public class ComplexTypeRecursorResult : List<ComplexTypeRecursorResultItem>
    {
        internal bool TryAdd(Type type, Type referrer = null, int recursion = 0)
        {
            if (this.Any(x => x.Type.Equals(type))) return false;
            Add(new ComplexTypeRecursorResultItem(type, referrer, recursion));
            return true;
        }

        public Type[] Entities()
        {
            return this.Select(x => x.Type).Distinct().ToArray();
        }
    }
}