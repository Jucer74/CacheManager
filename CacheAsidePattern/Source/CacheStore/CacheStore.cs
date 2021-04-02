using CacheAsidePattern.CacheStore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace CacheAsidePattern.CacheStore
{
    public class CacheStore : ICacheStore
    {
        private readonly Dictionary<string, TimeSpan> _expirationConfiguration;
        private readonly IMemoryCache _memoryCache;

        public CacheStore(IMemoryCache memoryCache, Dictionary<string, TimeSpan> expirationConfiguration)
        {
            _memoryCache = memoryCache;
            _expirationConfiguration = expirationConfiguration;
        }

        public void Add<TItem>(string key, TItem item)
        {
            var timespan = GetExpirationTime(key);

            if (timespan.HasValue)
            {
                _memoryCache.Set(key, item, timespan.Value);
            }
            else
            {
                _memoryCache.Set(key, item);
            }
        }

        public TItem Get<TItem>(string key) where TItem : class
        {
            if (_memoryCache.TryGetValue(key, out TItem value))
            {
                return value;
            }

            return null;
        }

        private TimeSpan? GetExpirationTime(string key)
        {
            if (_expirationConfiguration.ContainsKey(key))
            {
                return _expirationConfiguration[key];
            }

            return null;
        }
    }
}