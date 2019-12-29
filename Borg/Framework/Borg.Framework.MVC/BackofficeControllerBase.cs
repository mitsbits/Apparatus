using Borg.Framework.Dispatch;
using Borg.Framework.MVC.Services;
using Borg.Framework.Services.Background;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Framework.MVC
{
    public abstract class BackofficeControllerBase : Controller
    {
        protected IBackgroundRunner BackgroundRunner { get;set;}
        protected IMediator Dipsatcher { get; set; }
        protected IViewToStringRendererService ViewToStringRenderer { get; set; }
    }
}
