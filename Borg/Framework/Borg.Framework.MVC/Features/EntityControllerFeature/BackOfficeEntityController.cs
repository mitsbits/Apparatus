using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Borg.Framework.MVC.Features.EntityControllerFeature
{
    [BackOfficeEntityControllerName]
    public class BackOfficeEntityController<TModel> : BackofficeControllerBase where TModel : IIdentifiable, IDataState
    {
       

        public BackOfficeEntityController()
        {
           
        }
    }
}