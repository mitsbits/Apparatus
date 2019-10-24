using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Framework.Redirections
{
    public interface IRedirectEvaluator
    {
        Task<RedirectEvaluation> Evaluate(string input);
    }

    public interface IRedirectionRuleProvider
    {
        IEnumerable<IRedirectionRule> Rules { get; }
    }

    public interface IRedirectionRuleStore
    {
        Task AddOrUpdate(IRedirectionRule rule);
        Task Remove(IRedirectionRule rule);
    }

    public class RedirectEvaluator : IRedirectEvaluator, IRedirectionRuleStore
    {
        private readonly ILogger logger;
        public RedirectEvaluator(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        public Task AddOrUpdate(IRedirectionRule rule)
        {
            throw new System.NotImplementedException();
        }

        public Task<RedirectEvaluation> Evaluate(string input)
        {
            throw new System.NotImplementedException();
        }

        public Task Remove(IRedirectionRule rule)
        {
            throw new System.NotImplementedException();
        }
    }
}
