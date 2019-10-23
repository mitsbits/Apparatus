using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Framework.Redirections
{
    public interface IRedirectEvaluator
    {
        Task<RedirectEvaluation> Evaluate(string input);
    }
}
