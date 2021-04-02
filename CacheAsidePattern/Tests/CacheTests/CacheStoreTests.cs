using CacheAsidePattern.CacheStore;
using CacheAsidePattern.CacheStore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace CacheTests
{
    public class CacheStoreTests
    {
        private readonly IMemoryCache _memoryCache;

        public CacheStoreTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public void CacheStoreCanAddAnEntry()
        {
            // Arrange
            var keyName = "MyKeyName";
            var keyValue = "MyKeyValue";

            // Act
            ICacheStore _cache = new CacheStore(_memoryCache);
            _cache.Add<string>(keyName, keyValue);

            // Assert
            var cacheValue = _memoryCache.Get(keyName);
            Assert.NotNull(cacheValue);
            Assert.Equal(keyValue, cacheValue);
        }

        [Fact]
        public void CacheStoreCanAddAnEntryWithExpirationTime()
        {
            // Arrange
            var keyName = "MyKeyName";
            var keyValue = "MyKeyValue";
            var secondsToWait = 2;

            // Expire Configuration
            Dictionary<string, TimeSpan> expirationConfiguration = new Dictionary<string, TimeSpan>()
            {
                { "MyKeyName", TimeSpan.Parse($"00:00:0{secondsToWait}")}
            };

            // Act
            ICacheStore _cache = new CacheStore(_memoryCache, expirationConfiguration);
            _cache.Add<string>(keyName, keyValue);
            var cacheValue = _memoryCache.Get(keyName);
            Thread.Sleep(secondsToWait * 1000);
            var cacheValueAfterExpireTime = _memoryCache.Get<string>(keyName);

            // Assert
            Assert.NotNull(cacheValue);
            Assert.Equal(keyValue, cacheValue);
            Assert.Null(cacheValueAfterExpireTime);
        }

        [Fact]
        public void CacheStoreCanBeCreatedByInjection()
        {
            // Arrange-Act
            CacheStore myCache = new CacheStore(_memoryCache);

            // Assert
            Assert.NotNull(myCache);
        }

        [Fact]
        public void CacheStoreCanBeCreatedWithNullParameters()
        {
            // Arrange-Act
            CacheStore myCache = new CacheStore(null, null);

            // Assert
            Assert.NotNull(myCache);
        }

        [Fact]
        public void CacheStoreCanGetAnEntry()
        {
            // Arrange
            var keyName = "MyKeyName";
            var keyValue = "MyKeyValue";
            _memoryCache.Set(keyName, keyValue);

            // Act
            ICacheStore _cache = new CacheStore(_memoryCache);
            var cacheValue = _cache.Get<string>(keyName);

            // Assert
            Assert.NotNull(cacheValue);
            Assert.Equal(keyValue, cacheValue);
        }
    }
}