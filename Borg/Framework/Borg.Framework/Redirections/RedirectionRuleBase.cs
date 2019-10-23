using Borg.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace Borg.Framework.Redirections
{
    public abstract class RedirectionRuleBase : IRedirectionRule
    {
        protected RedirectionRuleBase(string pattern, RedirectStatusCode statusCode, PatternTestType patternTest, string target)
        {
            Pattern = Preconditions.NotEmpty(pattern, nameof(pattern));
            StatusCode = statusCode != RedirectStatusCode.Undefined ? statusCode : throw new ArgumentException(nameof(statusCode));
            PatternTest = patternTest;
            Target = Preconditions.NotEmpty(target, nameof(target));
        }
        public virtual string Pattern { get; }
        public virtual RedirectStatusCode StatusCode { get; }
        public PatternTestType PatternTest { get; }
        public virtual string Target { get; }
        public abstract Task<RedirectEvaluation> Evaluate(string input);
    }


}
