using Borg.Infrastructure.Core.DDD.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

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

    public interface ITenantProvider
    {
        ValueTask<IEnumerable<ITenant>> Tenants();
    }
}