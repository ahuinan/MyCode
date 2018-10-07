using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure
{
    /// <summary>
    /// 排序帮助类
    /// </summary>
    public static class SortHelper
    {
        /// <summary>
        /// 获取一个根据ASCII码排序算法的实体
        /// </summary>
        /// <returns></returns>
        public static ASCIISort GetASCIISort() 
        {
            return new ASCIISort();
        }
    }

    /// <summary>
    /// ASCII码排序算法
    /// </summary>
    public class ASCIISort : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var result = 0;
            //如果为空，则表示最小
            if (string.IsNullOrEmpty(x))
            {
                result = -1;
            }
            else if (string.IsNullOrEmpty(y))
            {
                result = 1;
            }
            else
            {
                //获取字符串的字符数组
                var xChars = x.ToCharArray();
                var yChars = y.ToCharArray();

                for (int i = 0; i < xChars.Length; i++)
                {
                    //将字符转换为ASCII码
                    var xc = (int)xChars[i];
                    
                    if (yChars.Length > i)
                    {
                        var yc = (int)yChars[i];
                        //根据ASCII码比较大小，如果有比较结果则退出，否则继续下一个比较
                        if (xc > yc)
                        {
                            result = 1;
                            break;
                        }
                        else if (xc < yc)
                        {
                            result = -1;
                            break;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                    else
                    {
                        //如果Y已经没有字符了，则表示比X短,短的字符排在前面
                        result = 1;
                        break;
                    }
                }

                //如果上面的比较结果为相等，并且X比Y短则表示X排在前面
                if (xChars.Length < yChars.Length && result == 0)
                {
                    result = -1;
                }
            }

            return result;
        }
    }
}
