using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Common
{
    public class PageResult<T>
    {
        /// <summary>
        /// 分页查询中的记录总数
        /// </summary>
        public int Total {
            get;
            set;
        }


        /// <summary>
        /// 分页查询中结果集合
        /// </summary>
        public List<T> DataList
        {
            get;
            set;
        }
    }
}
