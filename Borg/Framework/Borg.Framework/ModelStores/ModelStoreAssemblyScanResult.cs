using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Reflection.Discovery;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Borg.Framework.Cms.Contracts
{
    public class ModelStoreAssemblyScanResult : AssemblyScanResult
    {
        public ModelStoreAssemblyScanResult(Assembly assembly, List<Type> modelStoreTypes) : base(assembly, true, new string[0])
        {
            ModelStoreTypes = Preconditions.NotEmpty(modelStoreTypes, nameof(modelStoreTypes));
        }

        public ModelStoreAssemblyScanResult(Assembly assembly, string[] errors) : base(assembly, false, errors)
        {
            ModelStoreTypes = null;
        }
        public IEnumerable<Type> ModelStoreTypes { get; }
    }
}
