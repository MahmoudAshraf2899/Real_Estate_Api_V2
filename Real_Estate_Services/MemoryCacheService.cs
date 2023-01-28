using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Services
{
    public class MemoryCacheService : IMemoryCache
    {
        private readonly IMemoryCache _cache;
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public ICacheEntry CreateEntry(object key)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string key, T value, TimeSpan expiry)
        {
            _cache.Set(key, value, expiry);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public bool TryGetValue(object key, out object? value)
        {
            throw new NotImplementedException();
        }
    }
}
