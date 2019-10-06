using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        public IEnumerable<AssemblyScanResult> Get()
        {
            return explorerResult.Results;
        }
    }
}