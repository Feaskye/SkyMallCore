using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyMallCore.Core
{
    public interface IMemCache
    {
        T GetCache<T>(string cacheKey) where T : class;
        void SetCache<T>(T value, string cacheKey) where T : class;
        void SetCache<T>(T value, string cacheKey, DateTime expireTime) where T : class;
        void RemoveCache(string cacheKey);
        //void RemoveCache();
    }
}
