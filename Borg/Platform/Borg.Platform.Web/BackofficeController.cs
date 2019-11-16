using Borg.Framework.Cms.Contracts;
using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Platform.Web
{
    [Area(Constants.BackofficeAreaName)]
    public abstract class BackofficeController : ControllerBase
    {
        public BackofficeController(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }
        protected  ILogger Logger { get; }
    }

    public  class BackofficeModelController<TModel> : BackofficeController where TModel: IIdentifiable
    {
        public BackofficeModelController(ILoggerFactory loggerFactory, IModelStore<TModel> modelStore) :base(loggerFactory)
        {
            ModelStore = Preconditions.NotNull(modelStore, nameof(modelStore));
        }
        protected IModelStore<TModel> ModelStore { get; }
    }
}
