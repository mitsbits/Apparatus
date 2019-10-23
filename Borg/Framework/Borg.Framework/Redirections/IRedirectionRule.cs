using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Framework.Redirections
{
    public interface IRedirectionRule
    {
        string Pattern { get; }
        RedirectStatusCode StatusCode { get; }
        PatternTestType PatternTest { get; }
        string Target { get; }
        Task<RedirectEvaluation> Evaluate(string input);
    }


}
