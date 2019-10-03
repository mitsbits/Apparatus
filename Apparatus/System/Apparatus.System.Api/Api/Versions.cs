using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apparatus.System.Api.Api
{
    public class Versions
    {
        public static ApiVersion One => new ApiVersion(1);
    }

    public class ApiVersion
    {
        internal ApiVersion(int version)
        {
            if (version <= 0) throw new ArgumentOutOfRangeException($"{nameof(version)}:{version}");
            Version = version;
        }
        public int Version { get; }
    }
}
