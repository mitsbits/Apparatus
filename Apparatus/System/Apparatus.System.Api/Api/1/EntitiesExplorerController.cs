using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.AspNetCore.Mvc;

namespace Apparatus.System.Api.Controllers
{
    [Route("api/1/[controller]")]
    [ApiController]
    public class EntitiesExplorerController : ControllerBase
    {
        private readonly IAssemblyExplorerResult explorerResult;

        public EntitiesExplorerController(IAssemblyExplorerResult explorerResult)
        {
            this.explorerResult = Preconditions.NotNull(explorerResult, nameof(explorerResult));
        }

        public IAssemblyExplorerResult Get()
        {
            return explorerResult;
        }
    }
}