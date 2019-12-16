using Borg.Framework.Cms.BuildingBlocks;

namespace Borg.Platform.EF.SystemEntites
{
    public class Page : TreenodeActivatable, IPage
    {
        public string Title { get; protected set; }

        public string Slug { get; protected set; }
    }
}