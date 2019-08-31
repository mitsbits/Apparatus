using System;

namespace Borg.Framework.Cache
{
    public class CacheEntryOptions
    {
        private CacheEntryOptions(TimeSpan? expires)
        {
            Expires = expires;
        }

        public TimeSpan? Expires { get; }

        public static CacheEntryOptions Expiry(TimeSpan expires)
        {
            return new CacheEntryOptions(expires);
        }
    }
}