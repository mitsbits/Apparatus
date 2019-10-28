using Borg.Infrastructure.Core;
using System;
using System.Threading.Tasks;

namespace Borg.Framework.Redirections
{
    public class SimpleRedirectionRule : RedirectionRuleBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="pattern">the string to match</param>
        /// <param name="statusCode"></param>
        /// <param name="target"></param>
        /// <exception cref="UriFormatException" >simple redirection should always have a valid Uri for pattern and output</exception>
        public SimpleRedirectionRule(string pattern, RedirectStatusCode statusCode, string target) : base(pattern, statusCode, PatternTestType.Simple, target)
        {
            UriIn = new Uri(pattern);
            UriOut = new Uri(target);
        }

        protected Uri UriIn { get; }
        protected Uri UriOut { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException" >simple redirection requires input</exception>
        /// <exception cref="UriFormatException" >simple redirection should always have a valid Uri for input</exception>
        public override Task<RedirectEvaluation> Evaluate(string input)
        {
            var toMatch = new Uri(Preconditions.NotEmpty(input, nameof(input)));
            if (UriIn.Equals(toMatch))
            {
                var result = new RedirectEvaluation(UriOut.AbsoluteUri, StatusCode);
                return Task.FromResult(result);
            }
            return Task.FromResult(RedirectEvaluation.Negative);
        }
    }
}