using System;

namespace Borg.Framework.Modularity
{
    public interface ICanContextualize
    {
        bool ContextAcquired { get; }
    }

    public interface IUserSession : ICanContextualize
    {
        string SessionId { get; }
        DateTimeOffset SessionStart { get; }
        string UserIdentifier { get; }
        string UserName { get; }
        string DisplayName { get; }
        string Avatar { get; }

        bool IsAuthenticated();

        T Setting<T>(string key, T value);

        T Setting<T>(string key);

        void StartSession();
    }
}