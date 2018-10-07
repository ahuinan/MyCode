using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 默认值属性类
    /// </summary>
    public class DefaultValueProperty
    {
        /// <summary>
        /// 实体的属性名称
        /// </summary>
        public string EntityProp { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }
    }
}
