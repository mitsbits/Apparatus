using Borg.Framework.Dispatch;


namespace Borg.Framework.Caching
{
    public class EvictionRequest : IRequest
    {
        public string Key { get; }
    }
}