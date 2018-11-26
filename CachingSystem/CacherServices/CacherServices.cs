using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace CacherServices
{
    public class CacherServices : ICacherServices
    {
        private ObjectCache cache;
        private readonly ILoggingServices logger;

        public CacherServices(ILoggingServices logger, string memoryCacheName = null)
        {
            cache = string.IsNullOrEmpty(memoryCacheName) ? MemoryCache.Default : new MemoryCache(memoryCacheName);
            this.logger = logger ?? new LoggingServices();
        }

        public void ChangeTime(string tag, double newTimeLive)
        {
            if (string.IsNullOrEmpty(tag))
            {
                logger.Log($"Incorrect tag : {tag}");
                throw new ArgumentException("Incorrect tag");
            }

            if (newTimeLive < 0.0)
            {
                logger.Log($"Incorrect time live : {newTimeLive}");
                throw new ArgumentException("Incorrect time live");
            }

            cache.Set(cache.GetCacheItem(tag), new CacheItemPolicy
            { AbsoluteExpiration = DateTime.Now.AddMilliseconds(newTimeLive) });
        }

        public object GetCache(string tag)
        {
            if (cache.Contains(tag))
            {
                return cache.Get(tag);
            }

            logger.Log($"File passed away or not set, incorrect tag: {tag}");
            throw new ArgumentException("File passed away or not set");
        }

        public object RemoveCache(string tag)
        {
            if (!string.IsNullOrEmpty(tag) || cache.Contains(tag))
            {
                return cache.Remove(tag);
            }
            else
            {
                logger.Log($"File was deleted or was not created, tag: {tag}");
                throw new ArgumentNullException("file was deleted or was not created");
            }
        }

        public void SetCache(object obj, double timeLive, string tag)
        {

            if (obj == null || timeLive < 0.0 || string.IsNullOrEmpty(tag))
            {
                logger.Log("One of parameters isn't valid, check them:\n" +
                    $"Object: {obj}\nTag: {tag}\nTime live: {timeLive}");

                throw new ArgumentException("One of parameters isn't valid, check them:\n" +
                    $"Object: {obj}\nTag: {tag}\nTime live: {timeLive}");
            }

            CacheItemPolicy cacheInfo = new CacheItemPolicy();
            //                                             Milliseconds for tests
            cacheInfo.AbsoluteExpiration = DateTime.Now.AddMilliseconds(timeLive);

            cache.Add(tag, obj, cacheInfo);
            logger.Log($"Cache with tag: {tag} was created");
        }
    }
}
