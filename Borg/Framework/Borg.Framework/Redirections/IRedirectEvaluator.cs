using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

    public class RedirectEvaluator : IRedirectEvaluator, IRedirectionRuleStore, IRedirectionRuleProvider
    {
        private readonly ILogger logger;
        private static readonly Lazy<ConcurrentDictionary<string, IRedirectionRule>> rules = new Lazy<ConcurrentDictionary<string, IRedirectionRule>>(() => new ConcurrentDictionary<string, IRedirectionRule>());
        private ConcurrentDictionary<string, IRedirectionRule> Rules => rules.Value;
        IEnumerable<IRedirectionRule> IRedirectionRuleProvider.Rules => Rules.Values;

        public RedirectEvaluator(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        public Task AddOrUpdate(IRedirectionRule rule)
        {
            Rules.AddOrUpdate(rule.Pattern, rule, (key, updatevalue) => rule);
            return Task.CompletedTask;
        }

        public async Task<RedirectEvaluation> Evaluate(string input)
        {
            foreach (var rule in Rules.Values)
            {
                var result = await rule.Evaluate(input);
                if (result.ShouldRedirect)
                {
                    return result;
                }
            }
            return RedirectEvaluation.Negative;
        }

        public Task Remove(IRedirectionRule rule)
        {
            Rules.TryRemove(rule.Pattern, out rule);
            return Task.CompletedTask;
        }
    }
}