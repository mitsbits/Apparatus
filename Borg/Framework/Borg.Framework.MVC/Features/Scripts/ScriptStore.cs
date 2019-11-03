using Borg.Infrastructure.Core.Threading;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Borg.Framework.MVC.Features.Scripts
{
    public class ScriptStore : IScriptStore
    {
        private readonly ILogger logger;
        private readonly Lazy<ConcurrentBag<ScriptInfo>> bucket = new Lazy<ConcurrentBag<ScriptInfo>>(() => new ConcurrentBag<ScriptInfo>());
        private ConcurrentBag<ScriptInfo> Bucket => bucket.Value;
        private readonly AsyncLock @lock = new AsyncLock();

        public ScriptStore(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateForType(GetType());
        }

        public Task Add(ScriptInfo info)
        {
            Bucket.Add(info);
            return Task.CompletedTask;
        }

        public Task AddOnce(ScriptInfo info)
        {
            using (@lock.Lock())
            {
                var local = new List<ScriptInfo>(Bucket.Where(x => x.InfoType != info.InfoType || !x.Key.Equals(info.Key, StringComparison.InvariantCultureIgnoreCase)));
                Bucket.Clear();
                foreach (var item in local)
                {
                    Bucket.Add(item);
                }
                Bucket.Add(info);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ScriptInfo>> GetForPosition(ScriptInfoType infoType, ScriptPosition position)
        {
            var local = new List<ScriptInfo>(Bucket.Where(x => x.InfoType == infoType && x.Position == position));
            return Task.FromResult(local.AsEnumerable());
        }
    }
}