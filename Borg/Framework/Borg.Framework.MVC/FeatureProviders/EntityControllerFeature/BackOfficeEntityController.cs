﻿using Borg.Framework.MVC.FeatureProviders.EntityControllerFeature;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Borg.Framework.MVC.FeatureProviders
{
    [BackOfficeEntityControllerName]
    public class BackOfficeEntityController<TModel> : Controller where TModel : IIdentifiable
    {
        protected readonly IBackOfficeEntityServiceFacade<TModel> facade;
        public BackOfficeEntityController(IBackOfficeEntityServiceFacade<TModel> facade)
        {
            this.facade = Preconditions.NotNull(facade, nameof(facade));
        }
    }
}