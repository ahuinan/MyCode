using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Extensions;

namespace MyCode.Project.Infrastructure.Cache
{
    public class RedisCache : IMyCodeCacheService
    {
       
        private static ConnectionMultiplexer _connectionMultiplexer;

        private static IDatabase _database;

        /// <summary>
        /// Redis缓存前缀
        /// </summary>
        private static string _prefix = "";

        public RedisCache(string address,string prefix)
        {
            if (_connectionMultiplexer == null)
            {
                _connectionMultiplexer = ConnectionMultiplexer.Connect(address);
                _database = _connectionMultiplexer.GetDatabase(0);

                _prefix = prefix;
            }
        }

        #region GetCacheKey(得到带前缀的缓存key)
        private string GetCacheKey(string key)
        {
            return _prefix + key;
        }
        #endregion

        #region Incr(自增1，返回自增后的值)
        /// <summary>
        /// 自增1，返回自增后的值
        /// </summary>
        public long Incr(string key)
        {
            return _database.StringIncrement(GetCacheKey(key));
        }
        #endregion

        #region Get(根据key得到对象)
        public object Get(string key)
        {
            var cacheValue = _database.StringGet(GetCacheKey(key));

            if (cacheValue.HasValue)
            {
                var value = JsonHelper.ToObject(cacheValue);

                return value;
            }
            return null;
        }
        #endregion

        #region Get(得到缓存key的值)
        public T Get<T>(string key)
        {
            var value = Get(key);

            if (value == null) { return default(T); }

            return (T)value;

        }
        #endregion

        #region Set(设置缓存)
        public void Set(string key, object data, TimeSpan? cacheTime)
        {
            if (cacheTime == null || cacheTime.Value == TimeSpan.Zero) { cacheTime = new TimeSpan(0, 0, 30); }
            
            _database.StringSet(GetCacheKey(key), JsonHelper.ObjectToByte(data), cacheTime);
        }
        #endregion

        #region Remove(按key删除)
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            _database.KeyDelete(GetCacheKey(key), CommandFlags.HighPriority);
        }
        #endregion

        #region Exists(判断key是否存在)
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        public bool Exists(string key)
        {
            return _database.KeyExists(GetCacheKey(key));
        }
        #endregion
    }
}
