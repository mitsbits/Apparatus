using Borg.Infrastructure.Core.DDD.Contracts;

namespace Borg.Framework.Cms.BuildingBlocks
{
    public interface IPage : IActivatable, IHaveTitle, IHaveSlug, IHaveFullSlug, ITreeNode, IHaveHtmlContent
    {
    }
}