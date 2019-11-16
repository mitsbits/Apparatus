using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Borg.Framework.MVC.FeatureProviders.EntityControllerFeature
{
    public class BackOfficeEntityControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private IEnumerable<EntitiesAssemblyScanResult> source;

        public BackOfficeEntityControllerFeatureProvider(IAssemblyExplorerResult assemblyExplorerResult)
        {
            this.source = Preconditions.NotEmpty(assemblyExplorerResult.Results<EntitiesAssemblyScanResult>(x => x.Success), nameof(assemblyExplorerResult));
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var types = source.SelectMany(x => x.AllEntityTypes()).Distinct();

            foreach (var entityType in types)
            {
                var typeName = entityType.Name + "Controller";

                var dbtype = typeof(BorgDb);

                // Check to see if there is a "real" controller for this class
                if (!feature.Controllers.Any(t => t.Name == typeName))
                {
                    // Create a generic controller for this type
                    var controllerType = typeof(BackOfficeEntityController<,>).MakeGenericType(entityType, dbtype).GetTypeInfo();
                    feature.Controllers.Add(controllerType);
                }
            }
        }
    }
}
