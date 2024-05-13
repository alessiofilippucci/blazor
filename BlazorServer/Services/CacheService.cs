using Microsoft.Extensions.Caching.Memory;

namespace BlazorServer.Services
{
    public class CacheService
    {
        protected readonly IMemoryCache _cache;

        static object lockObject4Cache = new object();

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetOrSetCache<T>(string key, Func<T> func)
        {
            T data;

            lock (lockObject4Cache)
            {
                if (!_cache.TryGetValue(key, out data))
                {
                    data = func.Invoke();

                    var oneHour = new TimeSpan(1, 0, 0);
                    _cache.Set(key, data, oneHour);
                }
            }
            return data;
        }
    }
}
