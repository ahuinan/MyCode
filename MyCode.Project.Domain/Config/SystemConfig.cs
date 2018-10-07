using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Common;

namespace MyCode.Project.Domain.Config
{
    public  static class SystemConfig
    {


        /// <summary>
        /// 数据库连接路径
        /// </summary>
        public static string ConnectionMasterStr = WebConfigUtils.GetConnectionStringsInfo("MasterConn");

        /// <summary>
        /// 图片上传相关服务地址
        /// </summary>
        public static string PictureUrl = WebConfigUtils.GetAppSettingsInfo("PictureUrl");
        /// <summary>
        /// 短信服务的地址
        /// </summary>
        public static string SmsServiceUrl = WebConfigUtils.GetAppSettingsInfo("SmsServiceUrl");

        /// <summary>
        /// 菜鸟接口
        /// </summary>
        public static string DeliveryUrl = WebConfigUtils.GetAppSettingsInfo("DeliveryUrl");

        /// <summary>
        /// MongoDB的连接字符串
        /// </summary>
        public static string MongoDBConnectionString = WebConfigUtils.GetAppSettingsInfo("MongoDBConnectionString");

       /// <summary>
       ///  MongoDB的数据库名
       /// </summary>
        public static string MongoDBName = WebConfigUtils.GetAppSettingsInfo("MongoDBName");

        /// <summary>
        /// Redis的地址
        /// </summary>
        public static string RedisAddress = WebConfigUtils.GetAppSettingsInfo("RedisAddress");

        /// <summary>
        /// Redis缓存前缀
        /// </summary>
        public static string CachePrefix = WebConfigUtils.GetAppSettingsInfo("CachePrefix");

        /// <summary>
        /// JWT的key
        /// </summary>
        public static string JwtKey = WebConfigUtils.GetAppSettingsInfo("JwtKey");

        /// <summary>
        /// 是否输出sql
        /// </summary>
        public static bool IfOutputSql = WebConfigUtils.GetAppSettingsInfo("OutputSql") == "1" ? true:false;
    }
}
