using System;
using System.Runtime.Caching;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace CachingSystem
{
    public class Cacher
    {
        private ObjectCache cache;

        public Cacher()
            => cache = MemoryCache.Default;

        public object GetCache(string tag)
        {
            if (cache.Contains(tag))
            {
                return cache.Get(tag);
            }

            throw new ArgumentException("File passed away or not set");
    }

        public void SetCache(object obj, double timeLive, string tag)
        {
            if (obj == null || timeLive < 0.0 || string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException("One of parametrs isn't valid, check them:\n" +
                    $"Object: {obj}\nTag: {tag}\nTime live: {timeLive}");
            }

            CacheItemPolicy cacheInfo = new CacheItemPolicy();
            //                                             Milliseconds for tests
            cacheInfo.AbsoluteExpiration = DateTime.Now.AddMilliseconds(timeLive);

            cache.Add(tag, obj, cacheInfo);
        }
    }
}
