using Borg.Infrastructure.Core.DDD.Contracts;
using System.Collections.Generic;

namespace Borg
{
    public static class IIdentifiableExtensions
    {
        public static IEnumerable<string> HtmlDataAttributes(this IIdentifiable identifiable)
        {
            foreach (var key in identifiable.Keys.Keys)
            {
                yield return $" data-identifiable-{key.ToLower()}={identifiable.Keys[key]} ";
            }
        }
    }
}