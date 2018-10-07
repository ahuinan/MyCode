using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Enumeration;

namespace MyCode.Project.Infrastructure.Extensions
{
    /// <summary>
    /// 公共扩展
    /// </summary>
    public static partial class Extensions
    {

        #region IsEmpty(Guid是否为空)
        /// <summary>
        /// Guid 是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool IsEmpty(this Guid value)
        {
            if (value == Guid.Empty)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region IsEmpty(Guid? 是否为空)
        /// <summary>
        /// Guid? 是否为空
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool IsEmpty(this Guid? value)
        {
            if (value == null)
            {
                return true;
            }
            return IsEmpty(value.Value);
        }
        #endregion

        #region IsEmpty(判断 数组 是否为空)
        /// <summary>
        /// 判断 数组 是否为空
        /// </summary>
        /// <param name="array">数据</param>
        /// <returns></returns>
        public static bool IsEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }
        #endregion

        #region IsEmpty(判断 可变字符串 是否为空)
        /// <summary>
        /// 判断 可变字符串 是否为空
        /// </summary>
        /// <param name="sb">数据</param>
        /// <returns></returns>
        public static bool IsEmpty(this StringBuilder sb)
        {
            return sb == null || sb.Length == 0 || sb.ToString().IsEmpty();
        }
        #endregion

        #region IsEmpty(判断 泛型集合 是否为空)
        /// <summary>
        /// 判断 泛型集合 是否为空
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="list">数据</param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this ICollection<T> list)
        {
            return null == list || list.Count == 0;
        }
        #endregion

        #region IsEmpty(判断 迭代集合 是否为空)
        /// <summary>
        /// 判断 迭代集合 是否为空
        /// </summary>
        /// <typeparam name="T">泛型对象</typeparam>
        /// <param name="list">数据</param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return null == list || !list.Any();
        }
        #endregion

        #region IsEmpty(判断字典是否为空)
        /// <summary>
        /// 判断 字典 是否为空
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">数据</param>
        /// <returns></returns>
        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return null == dictionary || dictionary.Count == 0;
        }

        #endregion

        #region GetFirstDayOfMonth(获取指定日期的月份第一天)
        /// <summary>
        /// 获取指定日期的月份第一天
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>月份第一天</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        /// <summary>
        /// 获取指定日期的月份第一天，指定星期几
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="dayOfWeek">星期几</param>
        /// <returns>月份第一天</returns>
        public static DateTime GetFirstDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetFirstDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(1);
            return dt;
        }
        #endregion

        #region GetLastDayOfMonth(获取指定日期的月份最后一天)
        /// <summary>
        /// 获取指定日期的月份最后一天
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>最后一天</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, GetCountDaysOfMonth(date));
        }
        /// <summary>
        /// 获取指定日期的月份最后一天，指定星期几
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="dayOfWeek">星期几</param>
        /// <returns>最后一天</returns>
        public static DateTime GetLastDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetLastDayOfMonth();
            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(-1);
            return dt;
        }
        #endregion

        #region GetCountDaysOfMonth(获取月总天数)
        /// <summary>
        /// 获取月总天数
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>月总天数</returns>
        public static int GetCountDaysOfMonth(this DateTime date)
        {
            var nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1).AddDays(-1).Day;
        }
        #endregion

        #region ToDataTable(将List转换成数据表)
        /// <summary>
        /// 将List转换成数据表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">List集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this List<T> entities) where T : class
        {
            DataTable dt = new DataTable();            
            var properties = typeof(T).GetProperties().ToList();
            properties.ForEach(item =>
            {
                Type colType = item.PropertyType;
                if ((colType.IsGenericType) && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                dt.Columns.Add(new DataColumn(item.Name) {DataType = colType});
            });
            entities.ToList().ForEach(item =>
            {
                var dr = dt.NewRow();
                properties.ForEach(property =>
                {
                    var value = property.GetValue(item, null);
                    dr[property.Name] = value ?? DBNull.Value;
                });
                dt.Rows.Add(dr);
            });
            return dt;
        }
        #endregion

        #region SafeValue(安全返回值)
        /// <summary>
        /// 安全返回值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="value">可空值</param>
        /// <returns></returns>
        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }
        #endregion

        #region SafeString(安全转换为字符串)
        /// <summary>
        /// 安全转换为字符串，去除两端空格，当值为null时返回""
        /// </summary>
        /// <param name="input">输入值</param>
        /// <returns></returns>
        public static string SafeString(this object input)
        {
            return input == null ? string.Empty : input.ToString().Trim();
        }
        #endregion

        #region Value(获取成员值)
        /// <summary>
        /// 获取成员值
        /// </summary>
        /// <param name="instance">枚举实例</param>
        /// <returns></returns>
        public static int Value(this Enum instance)
        {
            return EnumHelper.GetValue(instance.GetType(), instance);
        }
        #endregion
    }
}
