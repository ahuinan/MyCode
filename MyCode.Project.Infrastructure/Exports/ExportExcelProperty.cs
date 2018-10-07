using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Exports
{
    /// <summary>
    /// 基本类型数据属性类
    /// </summary>
    public class ExportExcelProperty
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 实体的属性名称
        /// </summary>
        public string EntityProp { get; set; }

        /// <summary>
        /// 合并字段的分隔符号，默认为空字符串
        /// </summary>
        public string JoinPropChar { get; set; }

        public ExportExcelProperty()
        {
            JoinPropChar = "";
        }
    }
}
