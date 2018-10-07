using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Request.Common
{
    /// <summary>
    /// 导出请求
    /// </summary>
    public class ExportRequest
    {
        /// <summary>
        /// 导出文件类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public object Condition { get; set; }

        /// <summary>
        /// 商家ID,不用传
        /// </summary>
        public Guid? MerchantId { get; set; }

        /// <summary>
        /// 分销商ID,不用传
        /// </summary>
        public Guid? CustomerId { get; set; }
    }
}
