using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Collections;


namespace MyCode.Project.Infrastructure.Cache
{
    public class SystemCache:IMyCodeCacheService
    {
        private static System.Web.Caching.Cache cache;
      
		public SystemCache() {
            cache = HttpRuntime.Cache;
		}

        #region Set(设置绝对过期时间)
        public void Set(string key, object cache_object, TimeSpan? cacheTime = null) {

            if (cacheTime == null || cacheTime.Value == TimeSpan.Zero) { cacheTime = new TimeSpan(0, 0, 30); }

            //相对时间过期
            //cache.Insert(key,cache_object,null, System.Web.Caching.Cache.NoAbsoluteExpiration,expiration,priority,null);
            //绝对时间过期
            //cache.Insert(key, cache_object, null,DateTime.Now.addti,expiration, System.Web.Caching.Cache.NoSlidingExpiration);
            //var second = expiration.TotalSeconds;
            if (cache_object == null) { return; }

            cache.Insert(key, cache_object, null, DateTime.Now.AddSeconds(cacheTime.Value.TotalSeconds), System.Web.Caching.Cache.NoSlidingExpiration);

        }
        #endregion

        #region Get(根据key得到缓存值)
        public object Get(string key) {
            return cache.Get(key);
        }
        #endregion

        #region Get(获取一个具体的对象)
        public T Get<T>(string key)
        {
            var obj = Get(key);

            return (T)obj;
        }
        #endregion

        #region Delete(按Key删除)
        public void Delete(string key) {
            if (Exists(key)) {
                cache.Remove(key);
            }
        }
        #endregion

        #region Exists(判断缓存是否存在)
        public bool Exists(string key)
        {
            if (cache[key] != null)
            {
                return true;
            }
            else {
                return false;
            }
        }
        #endregion

        #region GetCacheKeys(取得缓存所有Key)
        public List<string> GetCacheKeys()
        {
            List<string> keys = new List<string>();
            IDictionaryEnumerator ca = cache.GetEnumerator();
            while (ca.MoveNext())
            {
                keys.Add(ca.Key.ToString());
            }
            return keys;
        }
        #endregion

    }
}
