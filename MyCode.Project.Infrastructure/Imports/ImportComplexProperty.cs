using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 复杂类型数据属性类
    /// </summary>
    public class ImportComplexProperty:ImportProperty
    {
        /// <summary>
        /// 复杂属性的名称
        /// </summary>
        public string ComplexPropName { get; set; }

        /// <summary>
        /// 复杂属性的类型
        /// </summary>
        public Type ComplexPropType { get; set; }

        /// <summary>
        /// 子属性类型
        /// </summary>
        public ImportChildProperty ChildProperty { get; set; }

    }
}
