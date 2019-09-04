using Borg.Infrastructure.Core.Collections;
using Borg.Infrastructure.Core.DDD.Contracts;

namespace Borg.Framework.Cms.Contracts
{
    public interface ICmsContentPage : IHaveTitle
    {
    }

    public interface ICmsEntityPage<TEntity> : ICmsContentPage where TEntity : IEntity
    {
        TEntity Data { get; }
    }

    public interface ICmsEntityGridPage<TEntity> : ICmsContentPage where TEntity : IEntity
    {
        IPagedResult<TEntity> Data { get; }
    }
}