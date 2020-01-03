using Borg.Framework.Cms.Contracts;
using Borg.Framework.EF.Discovery.AssemblyScanner;
using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Platform.EF
{
    public class BackOfficeEntityControllerConstraint : IRouteConstraint
    {
        private List<string> Types = new List<string>();

        public BackOfficeEntityControllerConstraint(IEnumerable<IAssemblyExplorerResult> result)
        {
            var results = result.SelectMany(x => x.Results<BorgDbAssemblyScanResult>()).Distinct().SelectMany(x => x.DbEntities).Distinct();
            
            foreach (var kv  in results)
            {
                foreach(var t in kv.Value)
                {
                    if (!Types.Contains(t.Name))
                    {
                        Types.Add(t.Name);
                    }
                }
            
            }
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
                if (values.TryGetValue(routeKey, out routeValue))
                {
                    return Types.Any(x => x.Equals(routeValue.ToString(), StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return false;
        }
    }
}