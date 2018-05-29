using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Web;


namespace SkyMallCore.Core
{
    public class MemCache : IMemCache
    {
        private IMemoryCache _MemCache;
        public MemCache(IMemoryCache memCache)
        {
            _MemCache = memCache;
        }

        public T GetCache<T>(string cacheKey) where T : class
        {
            var val = default(T);
            if (_MemCache.TryGetValue<T>(cacheKey,out val))
            {
                return val;
            }
            return val;
        }
        public void SetCache<T>(T value, string cacheKey) where T : class
        {
            _MemCache.Set<T>(cacheKey, value,new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }
        public void SetCache<T>(T value, string cacheKey, DateTime expireTime) where T : class
        {
            _MemCache.Set<T>(cacheKey, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(expireTime));
        }
        public void RemoveCache(string cacheKey)
        {
            _MemCache.Remove(cacheKey);
        }
        //public void RemoveCache()
        //{
        //    IDictionaryEnumerator CacheEnum = MemCache.GetEnumerator();
        //    while (CacheEnum.MoveNext())
        //    {
        //        MemCache.Remove(CacheEnum.Key.ToString());
        //    }
        //}
    }
}
