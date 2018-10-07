using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure
{
    /// <summary>
    /// 返回结果的枚举
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 找不到数据
        /// </summary>
        NoResultFound = 0,
        /// <summary>
        /// 发生错误
        /// </summary>
        Error = -1
    }
}
