using CacheAsidePattern.CacheStore;
using CacheAsidePattern.CacheStore.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}