using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Extensions;

namespace MyCode.Project.Infrastructure.Format
{
    public static class Money
    {
        /// <summary>
        /// 保留两位小数,四舍五入
        /// </summary>
        public static string ToTwoPoint(this decimal value)
        {
            return value.ToString("f2");
        }

 
    }
}
