using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Apparatus.System.Backoffice.Areas.Apparatus.Components
{
    public class MainNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {

            return View();
        }
    }
}
