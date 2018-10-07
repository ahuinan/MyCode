using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 二维数据导入
    /// </summary>
    public class ImportSplitProperty:ImportProperty
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
        /// 分隔符，默认为逗号
        /// </summary>
        public char Speparator { get; set; }

        /// <summary>
        /// 子属性类型
        /// </summary>
        public ImportChildProperty ChildProperty { get; set; }

        /// <summary>
        /// 初始化一个<see cref="ImportSplitProperty"/>类型的实例
        /// </summary>
        public ImportSplitProperty()
        {
            this.Speparator = ',';
        }
    }
}
