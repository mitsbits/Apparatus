using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System.Text;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    /// <summary>
    /// Defines a class that can be uniquely identified in a collection by providing a <see cref="CompositeKey"/>
    /// </summary>
    public interface IIdentifiable
    {
        CompositeKey Keys { get; }
    }

    /// <summary>
    /// Marker interface. Defines a class that persists and reads state.
    /// </summary>
    public interface IDataState { }
}

namespace Borg
{
    public static class IIdentifiableExtensions
    {
        public static string StringKey(this IIdentifiable identifiable)
        {
            var builder = new StringBuilder($"[{identifiable.GetType().FullName}]");
            foreach (var key in identifiable.Keys.Keys)
            {
                builder.Append($"[{key},{identifiable.Keys[key]}]");
            }
            return builder.ToString();
        }
    }
}