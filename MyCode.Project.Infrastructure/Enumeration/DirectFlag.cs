using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Enumeration
{
    /// <summary>
    /// 流水方向
    /// </summary>
    public enum DirectFlag
    {
        /// <summary>
        /// 减少
        /// </summary>
        [Description("减少")]
        Reduce=-1,
        /// <summary>
        /// 增加
        /// </summary>
        [Description("增加")]
        Increase=1
    }
}
