using Borg.Infrastructure.Core.Reflection.Discovery;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Borg;
using System.Linq;
using System.Collections.Generic;

namespace Apparatus.System.Backoffice.Areas.Apparatus.Components
{
    public class MainNavigationViewComponent : ViewComponent
    {
        private readonly IEnumerable<IAssemblyExplorerResult> result;
        public MainNavigationViewComponent(IEnumerable<IAssemblyExplorerResult> result)
        {
            this.result = result;
        }
        public IViewComponentResult Invoke()
        {
            var localResults = result.SelectMany(r=> r.Results<Borg.Framework.EF.Discovery.AssemblyScanner.BorgDbAssemblyScanResult>());

            var outputs = new menu();
            foreach (var db in localResults.SelectMany(x => x.DbEntities.Keys)) {

                var section = new section();
                var ul = new ul();
                var dbli = new label() { display = db.Name, tooltip = db.Name };

                var enitiesul = new ul();
                foreach(var ent in localResults.SelectMany(x=> x.DbEntities[db]).Distinct())
                {

                    var target = Url.RouteUrl("backofficeentity", new { Controller = ent.Name, area = "apparatus", dbcontext = db.Name });
                    enitiesul.lis.Add(new anchor() { display = ent.Name, href = target });
                }
                dbli.ul = enitiesul;
                ul.lis.Add(dbli);
                section.uls.Add(ul);
                outputs.sections.Add(section);

            }
            return  View(outputs);
        }
    }
}