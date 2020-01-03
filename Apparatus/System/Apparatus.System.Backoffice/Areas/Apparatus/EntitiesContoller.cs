using Borg.Framework.EF.Discovery.AssemblyScanner;
using Borg.Framework.MVC;
using Borg.Framework.MVC.Features.EntityControllerFeature;
using Borg.Infrastructure.Core.DDD.Contracts;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Borg;
using System.Reflection;

namespace Apparatus.System.Backoffice.Areas.Apparatus
{
    [Area("apparatus")]
    [BackOfficeEntityControllerName]
    public class EntitiesContoller<TModel> : BackOfficeEntityController<TModel> where TModel : IIdentifiable, IDataState
    {
        public EntitiesContoller() : base()
        {
        }

        public ActionResult Index()
        {
            var t = this.RouteData;
            return View("~/Areas/Apparatus/Views/Entity/Grid.cshtml");
        }
    }

    public class EntityControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private IEnumerable<BorgDbAssemblyScanResult> source;

        public EntityControllerFeatureProvider(IEnumerable<IAssemblyExplorerResult> result)
        {
            this.source = result.SelectMany(x => x.Results<BorgDbAssemblyScanResult>()).Distinct();
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var types = source.SelectMany(x => x.DbEntities.Values).SelectMany(x => x).Distinct();

            foreach (var entityType in types)
            {
                var typeName = entityType.Name + "Controller";

                // Check to see if there is a "real" controller for this class
                if (!feature.Controllers.Any(t => t.Name == typeName))
                {
                    // Create a generic controller for this type
                    var controllerType = typeof(EntitiesContoller<>).MakeGenericType(entityType).GetTypeInfo();
                    feature.Controllers.Add(controllerType);
                }
            }
        }
    }
}
