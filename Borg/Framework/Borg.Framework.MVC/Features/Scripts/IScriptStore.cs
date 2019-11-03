using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borg.Framework.MVC.Features.Scripts
{
    public interface IScriptStore
    {
        Task Add(ScriptInfo info);

        Task AddOnce(ScriptInfo info);

        Task<IEnumerable<ScriptInfo>> GetForPosition(ScriptInfoType infoType, ScriptPosition position);
    }
}