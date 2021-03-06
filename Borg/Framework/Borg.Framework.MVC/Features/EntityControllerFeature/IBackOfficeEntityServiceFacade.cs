﻿using Borg.Framework.Cms.Contracts;
using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.Extensions.Logging;

namespace Borg.Framework.MVC.Features.EntityControllerFeature
{
    public interface IBackOfficeEntityServiceFacade<TModel> where TModel : IIdentifiable, IDataState
    {
        ILoggerFactory LoggerFactory { get; }
        IModelStore<TModel> Store { get; }
    }
}