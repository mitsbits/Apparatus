using System.Collections.Generic;
using System.Reflection;

namespace Borg.Infrastructure.Core.Reflection.Discovery
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}