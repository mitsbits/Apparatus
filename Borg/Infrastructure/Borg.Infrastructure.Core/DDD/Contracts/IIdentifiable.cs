using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.DDD.ValueObjects;
using System.Text;

namespace Borg.Infrastructure.Core.DDD.Contracts
{
    public interface IIdentifiable
    {
        CompositeKey Keys { get; }
    }

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