using MediatR;

namespace Borg.Framework.Caching
{
    public class EvictionRequest : IRequest
    {
        public string Key { get; }
    }
}