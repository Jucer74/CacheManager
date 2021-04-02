namespace CacheAsidePattern.CacheStore.Interfaces
{
    public interface ICacheStore
    {
        void Add<TItem>(string key, TItem item);

        TItem Get<TItem>(string key) where TItem : class;
    }
}