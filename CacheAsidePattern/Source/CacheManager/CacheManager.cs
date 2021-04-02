using CacheAsidePattern.CacheManager.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheAsidePattern.CacheManager
{
    public class CacheManager : ICacheManager
    {
        private readonly Dictionary<string, TimeSpan> _expirationConfiguration;
        private readonly IMemoryCache _memoryCache;

        public CacheManager(IMemoryCache cache, Dictionary<string, TimeSpan>? expirationConfiguration = null)
        {
            if (cache is null)
            {
                _memoryCache = new MemoryCache(new MemoryCacheOptions());
            }
            else
            {
                _memoryCache = cache;
            };

            _expirationConfiguration = expirationConfiguration;
        }

        public TItem GetOrAdd<TItem>(string key, Func<TItem> getFunction)
        {
            if (!_memoryCache.TryGetValue(key, out TItem cacheEntry))
            {
                cacheEntry = getFunction();

                TimeSpan? expireIn = GetExpirationTime(key);

                if (expireIn.HasValue)
                {
                    _memoryCache.Set(key, cacheEntry, expireIn.Value);
                }
                else
                {
                    _memoryCache.Set(key, cacheEntry);
                }
            }

            return cacheEntry;
        }

        public async Task<TItem> GetOrAddAsync<TItem>(string key, Func<Task<TItem>> getFunction)
        {
            if (!_memoryCache.TryGetValue(key, out TItem cacheEntry))
            {
                cacheEntry = await getFunction();

                TimeSpan? expireIn = GetExpirationTime(key);

                if (expireIn.HasValue)
                {
                    _memoryCache.Set(key, cacheEntry, expireIn.Value);
                }
                else
                {
                    _memoryCache.Set(key, cacheEntry);
                }
            }

            return cacheEntry;
        }

        public void Remove<TItem>(string key)
        {
            if (_memoryCache.TryGetValue(key, out TItem cacheEntry))
            {
                _memoryCache.Remove(key);
            }
        }

        private TimeSpan? GetExpirationTime(string key)
        {
            if (!(_expirationConfiguration is null) && _expirationConfiguration.ContainsKey(key))
            {
                return _expirationConfiguration[key];
            }

            return null;
        }
    }
}