using Microsoft.AspNetCore.Mvc;

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