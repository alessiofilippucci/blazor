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

        public Task<(bool succeeded, T? value)> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                return Task.FromResult((succeeded: false, value: default(T?)));

            lock (lockObject4Cache)
            {
                var succeeded = _cache.TryGetValue(key, out T? result);

                if (succeeded)
                    return Task.FromResult((succeeded, value: succeeded ? result : default));
                else
                    return Task.FromResult((succeeded, value: default(T?)));
            }
        }

        public Task SetAsync<T>(string key, T value, int minutesToCache = 60)
        {
            if (string.IsNullOrEmpty(key))
                return Task.FromResult((succeeded: false, value: default(T?)));

            lock (lockObject4Cache)
            {
                var relativeTime = new TimeSpan(hours: 0, minutes: minutesToCache, seconds: 0);
                _cache.Set(key, value, relativeTime);
                return Task.CompletedTask;
            }
        }

        public Task RemoveAsync(params string[] keys)
        {
            lock (lockObject4Cache)
            {
                if (keys == null || keys.Length < 1) return Task.CompletedTask;

                var numberOfKeys = keys.Length;

                for (int i = 0; i < numberOfKeys; i++)
                    _cache.Remove(keys[i]);

                return Task.CompletedTask;
            }
        }
    }
}
