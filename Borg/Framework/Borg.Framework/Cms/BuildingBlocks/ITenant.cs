using Borg.Infrastructure.Core.DDD.Contracts;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public interface ISilo
    {
    }

    public interface ITenant : ISilo, IHaveName, IHaveDescription
    {
    }

    public interface ILanguage : ISilo
    {
    }
}