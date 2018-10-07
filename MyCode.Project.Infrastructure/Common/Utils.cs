using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Constant;
using MyCode.Project.Infrastructure.Extensions;

namespace MyCode.Project.Infrastructure
{
    
    /// <summary>
    /// 
    /// </summary>
    public class Utils
    {
        #region 获取业务编号
        /// <summary>
        /// 获取单据编号
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <returns></returns>
        public static string GetBillNo(string prefix)
        {
            var num = (new Random().Next(99) + 1).ToString("00");
            return prefix + DateTime.Now.ToString("yyMMddHHmmssfff") + num;
        }
        #endregion

        #region GetRandNum(从一个范围得到一个随机的数字)
        public static int GetRandNum(int min,int max)
        {
            return new Random().Next(min, max);
        }
        #endregion
    }
}
