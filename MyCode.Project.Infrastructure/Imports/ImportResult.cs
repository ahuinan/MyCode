using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 导入结果实体
    /// </summary>
    public class ImportResult
    {
        /// <summary>
        /// 新增记录数
        /// </summary>
        public int InsertCount { get; set; }

        /// <summary>
        /// 更新记录数
        /// </summary>
        public int UpdateCount { get; set; }
    }
}
