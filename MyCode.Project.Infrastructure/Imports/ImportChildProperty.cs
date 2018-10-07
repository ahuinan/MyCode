using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 实体的子属性
    /// </summary>
    public class ImportChildProperty
    {
        /// <summary>
        /// 实体的属性名称
        /// </summary>
        public string EntityProp { get; set; }

        /// <summary>
        /// 属性的类型
        /// </summary>
        public Type PropType { get; set; }
    }
}
