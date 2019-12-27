using Borg.Framework.Cms.Contracts;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Framework.MVC.Features.EntityControllerFeature
{
    public class BackOfficeEntityControllerConstraint : IRouteConstraint
    {
        private List<string> Types = new List<string>();

        public BackOfficeEntityControllerConstraint()
        {
            //var results = assemblyExplorerResult.Results<BorgDbAssemblyScanResult>().Where(x => x.Success).ToList();
            //Types.AddRange(results.SelectMany(x => x.ModelStoreTypes).Distinct().Select(x => x.Name));
        }

        public bool Match(HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            object routeValue;
            if (routeDirection == RouteDirection.IncomingRequest)
            {
                if (values.TryGetValue(routeKey, out routeValue))
                {
                    return Types.Any(x => x.Equals(routeValue.ToString(), StringComparison.InvariantCultureIgnoreCase));
                }
            }
            else
            {
            }

            return false;
        }
    }
}