using Borg.Infrastructure.Core;
using Borg.Infrastructure.Core.Threading;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borg.Framework.MVC.Features.Js
{
    [HtmlTargetElement("script")]
    public class ScriptTagHelper : ScriptTagHelperBase
    {
        public ScriptTagHelper(IScriptStore store) : base(store)
        {
        }

        public string Key { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return base.ProcessAsync(context, output);
        }
    }

    public class ScriptRenderTagHelper : ScriptTagHelperBase
    {
        public ScriptRenderTagHelper(IScriptStore store) : base(store)
        {
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return base.ProcessAsync(context, output);
        }
    }

    public abstract class ScriptTagHelperBase : TagHelper
    {
        protected readonly IScriptStore store;

        protected ScriptTagHelperBase(IScriptStore store)
        {
            this.store = Preconditions.NotNull(store, nameof(store));
        }

        public ScriptPosition ScriptPosition { get; set; } = ScriptPosition.BodyBeforeEnd;
    }

    public enum ScriptPosition
    {
        Inline,
        Head,
        HeadBeforeEnd,
        BodyAfterStart,
        BodyBeforeEnd
    }

    public class ScriptInfo
    {
        internal string Src { get; set; }
        internal string Key { get; set; }
        internal HtmlString InlineContent { get; set; }
        internal ScriptPosition Position { get; set; } = ScriptPosition.BodyBeforeEnd;
        public override string ToString()
        {
            var builder = new StringBuilder();
            if (!Key.IsNullOrWhiteSpace())
            {
                builder.Append($"[{Key}]");
            }
            builder.Append($"[{Position.ToString()}]");
            if (!string.IsNullOrWhiteSpace(Src))
            {
                builder.Append($"[src:{Src}]");
            }
            else
            {
                builder.Append($"[src:inline]");
            }
            return builder.ToString();
        }
    }

    public interface IScriptStore
    {
        Task Add(ScriptInfo info);

        Task AddOnce(ScriptInfo info);

        Task<IEnumerable<ScriptInfo>> GetForPosition(ScriptPosition position);
    }

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
                var local = new List<ScriptInfo>(Bucket.Where(x => !x.Key.Equals(info.Key, StringComparison.InvariantCultureIgnoreCase)));
                Bucket.Clear();
                foreach (var item in local)
                {
                    Bucket.Add(item);
                }
                Bucket.Add(info);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<ScriptInfo>> GetForPosition(ScriptPosition position)
        {
            var local = new List<ScriptInfo>(Bucket.Where(x => x.ScriptPosition == position);
            return Task.FromResult(local.AsEnumerable());
        }
    }
}