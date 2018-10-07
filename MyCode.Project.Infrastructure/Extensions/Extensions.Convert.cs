using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Common;

namespace MyCode.Project.Infrastructure.Extensions
{
    public static partial class Extensions
    {


        #region ToBool(转换为bool)
        /// <summary>
        /// 转换为bool
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static bool ToBool(this string obj)
        {
            return Conv.ToBool(obj);
        }

        /// <summary>
        /// 转换为可空bool
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static bool? ToBoolOrNull(this string obj)
        {
            return Conv.ToBoolOrNull(obj);
        }
        #endregion

        #region ToInt(转换为int)
        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static int ToInt(this string obj)
        {
            return Conv.ToInt(obj);
        }

        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static int? ToIntOrNull(this string obj)
        {
            return Conv.ToIntOrNull(obj);
        }
        #endregion

        #region ToLong(转换为long)
        /// <summary>
        /// 转换为long
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static long ToLong(this string obj)
        {
            return Conv.ToLong(obj);
        }

        /// <summary>
        /// 转换为可空long
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static long? ToLongOrNull(this string obj)
        {
            return Conv.ToLongOrNull(obj);
        }
        #endregion

        #region ToDouble(转换为double)

        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static double ToDouble(this string obj)
        {
            return Conv.ToDouble(obj);
        }

        /// <summary>
        /// 转换为可空double
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static double? ToDoubleOrNull(this string obj)
        {
            return Conv.ToDoubleOrNull(obj);
        }
        #endregion

        #region ToDecimal(转换为decimal)

        /// <summary>
        /// 转换为decimal
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string obj)
        {
            return Conv.ToDecimal(obj);
        }

        /// <summary>
        /// 转换为可空decimal
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static decimal? ToDecimalOrNull(this string obj)
        {
            return Conv.ToDecimalOrNull(obj);
        }
        #endregion

        #region ToDate(转换为日期)
        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static DateTime ToDate(this string obj)
        {
            return Conv.ToDate(obj);
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static DateTime? ToDateOrNull(this string obj)
        {
            return Conv.ToDateOrNull(obj);
        }
        #endregion

        #region ToGuid(转换为Guid)

        /// <summary>
        /// 转化为Guid
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static Guid ToGuid(this string obj)
        {
            return Conv.ToGuid(obj);
        }

        /// <summary>
        /// 转换为可空Guid
        /// </summary>
        /// <param name="obj">数据</param>
        /// <returns></returns>
        public static Guid? ToGuidOrNull(this string obj)
        {
            return Conv.ToGuidOrNull(obj);
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="obj">数据，范例："83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A"</param>
        /// <returns></returns>
        public static List<Guid> ToGuidList(this string obj)
        {
            return Conv.ToGuidList(obj);
        }

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="obj">字符串集合</param>
        /// <returns></returns>
        public static List<Guid> ToGuidList(this IList<string> obj)
        {
            if (obj == null)
            {
                return new List<Guid>();
            }
            return obj.Select(t => t.ToGuid()).ToList();
        }
        #endregion
    }
}
