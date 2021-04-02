using CacheAsidePattern.CacheManager;
using CacheAsidePattern.CacheManager.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CacheTests
{
    public class CacheManagerTests
    {
        private readonly IMemoryCache _memoryCache;

        public CacheManagerTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public void CacheManagerCanBeCreatedByInjection()
        {
            // Arrange-Act
            CacheManager myCache = new CacheManager(_memoryCache);

            // Assert
            Assert.NotNull(myCache);
        }

        [Fact]
        public void CacheManagerCanBeCreatedWithNullParameters()
        {
            // Arrange-Act
            CacheManager myCache = new CacheManager(null, null);

            // Assert
            Assert.NotNull(myCache);
        }

        [Fact]
        public async Task GetOrAddAsyncExistValue()
        {
            // Arrange
            var keyName = "MyKeyName";
            var keyValue = "MyKeyValue";
            Task<string> getFunction() => AsyncFunction(keyValue);
            _memoryCache.Set(keyName, keyValue);

            // Act
            ICacheManager _cache = new CacheManager(_memoryCache);
            var cacheValue = await _cache.GetOrAddAsync<string>(keyName, getFunction);

            // Asserts
            Assert.NotNull(cacheValue);
            Assert.Equal(keyValue, cacheValue);
        }

        [Fact]
        public async Task GetOrAddAsyncNotExistValue()
        {
            // Arrange
            var keyName = "MyKeyName";
            var keyValue = "MyKeyValue";
            Task<string> getFunction() => AsyncFunction(keyValue);

            // Act
            ICacheManager _cache = new CacheManager(_memoryCache);
            var cacheValue = await _cache.GetOrAddAsync<string>(keyName, getFunction);

            // Asserts
            Assert.NotNull(cacheValue);
            Assert.Equal(keyValue, cacheValue);
        }

        [Fact]
        public async Task GetOrAddAsyncNotExistValueAndExpireInNSecondsAsync()
        {
            // Arrange
            var keyName = "MyKeyName";
            var keyValue = "MyKeyValue";
            var secondsToWait = 2;
            Task<string> getFunction() => AsyncFunction(keyValue);
            // Expire Configuration
            Dictionary<string, TimeSpan> expirationConfiguration = new Dictionary<string, TimeSpan>()
            {
                { "MyKeyName", TimeSpan.Parse($"00:00:0{secondsToWait}")}
            };

            // Act
            ICacheManager _cache = new CacheManager(_memoryCache, expirationConfiguration);
            var cacheValue = await _cache.GetOrAddAsync<string>(keyName, getFunction);
            Thread.Sleep(secondsToWait * 1000);
            var cacheValueAfterExpireTime = _memoryCache.Get<string>(keyName);

            // Asserts
            Assert.NotNull(cacheValue);
            Assert.Equal(keyValue, cacheValue);
            Assert.Null(cacheValueAfterExpireTime);
        }

        [Fact]
        public void GetOrAddNotExistValue()
        {
            // Arrange
            var keyName = "MyKeyName";
            var keyValue = "MyKeyValue";

            // Act
            ICacheManager _cache = new CacheManager(_memoryCache);
            var cacheValue = _cache.GetOrAdd(keyName, () => { return keyValue; });

            // Asserts
            Assert.NotNull(cacheValue);
            Assert.Equal(keyValue, cacheValue);
        }

        [Fact]
        public void GetOrAddNotExistValueAndExpireInNSeconds()
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
            ICacheManager _cache = new CacheManager(_memoryCache, expirationConfiguration);
            var cacheValue = _cache.GetOrAdd(keyName, () => { return keyValue; });
            Thread.Sleep(secondsToWait * 1000);
            var cacheValueAfterExpireTime = _memoryCache.Get<string>(keyName);

            // Asserts
            Assert.NotNull(cacheValue);
            Assert.Equal(keyValue, cacheValue);
            Assert.Null(cacheValueAfterExpireTime);
        }

        [Fact]
        public void GetOrAddWithExistValue()
        {
            // Arrange
            var keyName = "MyKeyName";
            var keyValue = "MyKeyValue";
            _memoryCache.Set(keyName, keyValue);

            // Act
            ICacheManager _cache = new CacheManager(_memoryCache);
            var cacheValue = _cache.GetOrAdd(keyName, () => { return keyValue; });

            // Asserts
            Assert.NotNull(cacheValue);
            Assert.Equal(keyValue, cacheValue);
        }

        [Fact]
        public void RemoveAndExistEntry()
        {
            // Arrange
            var keyName = "MyKeyName";
            var keyValue = "MyKeyValue";
            _memoryCache.Set(keyName, keyValue);

            // Act
            ICacheManager _cache = new CacheManager(_memoryCache);
            _cache.Remove<string>(keyName);

            // Asserts
            var cacheValue = _memoryCache.Get<string>(keyName);
            Assert.Null(cacheValue);
        }

        [Fact]
        public void RemoveAndNotExistEntry()
        {
            // Arrange
            var keyName = "MyKeyName";

            // Act
            ICacheManager _cache = new CacheManager(_memoryCache);
            _cache.Remove<string>(keyName);

            // Asserts
            var cacheValue = _memoryCache.Get<string>(keyName);
            Assert.Null(cacheValue);
        }

        private async Task<string> AsyncFunction(string value)
        {
            await Task.Yield();
            return value;
        }
    }
}