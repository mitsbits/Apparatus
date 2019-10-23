namespace Borg.Framework.Redirections
{
    public abstract class RegexRedirectionRule : RedirectionRuleBase
    {

        public RegexRedirectionRule(string pattern, RedirectStatusCode statusCode, string target) : base(pattern, statusCode, PatternTestType.Regex, target)
        {
        }

    }


}
