using Borg.Infrastructure.Core.DDD.Contracts;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public interface ISilo
    {
    }

    public interface ITenant : ISilo, IHaveName
    {
    }

    public interface ILanguage : ISilo
    {
    }
}