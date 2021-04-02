using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CacheAsidePattern.CacheManager.Interfaces
{
    public interface ICacheManager
    {
        TItem GetOrAdd<TItem>(string key, Func<TItem> getFunction);

        Task<TItem> GetOrAddAsync<TItem>(string key, Func<Task<TItem>> getFunction);

        void Remove<TItem>(string key);
    }
}