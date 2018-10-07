using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Cache
{
    public interface IMyCodeCacheService
    {

        /// <summary>
        /// 根据key得到缓存值
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 根据缓存Key返回一个具体的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="cache_object">缓存对象</param>
        /// <param name="expiration">过期时间</param>
        void Set(string key, object cache_object, TimeSpan? expiration=null);

        /// <summary>
        /// 根据key删除一个缓存
        /// </summary>
        /// <param name="key"></param>
        void Delete(string key);

        /// <summary>
        /// 判断是否存在缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);

    }
}
