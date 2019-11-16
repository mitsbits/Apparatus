using Borg.Framework.Cms.Contracts;
using Borg.Infrastructure.Core.DDD.Contracts;
using Microsoft.Extensions.Logging;

namespace Borg.Framework.MVC.FeatureProviders
{
    public class BackOfficeEntityServiceFacade<TModel> : IBackOfficeEntityServiceFacade<TModel> where TModel : IIdentifiable
    {
        public BackOfficeEntityServiceFacade(ILoggerFactory loggerFactory, IModelStore<TModel> store)
        {
            LoggerFactory = loggerFactory;
            Store = store;
        }

        public ILoggerFactory LoggerFactory { get; }

        public IModelStore<TModel> Store { get; }
    }
}