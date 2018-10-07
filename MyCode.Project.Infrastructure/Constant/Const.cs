using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Constant
{
    /// <summary>
    /// 系统全局配置代码常量类,
    /// 代码的编码规则是前面第一位数是大类，
    /// 初步分为1：系统控制，2：订单业务，3：进销存业务，4：配发货业务，5：档案相关，9：定制
    /// 后面需要再做优化
    /// 第二第三位数是子分类，例如01代表权限数据
    /// 后面三位数是流水
    /// </summary>
    public class Const
    {

		/// <summary>
		/// 标准模板商家
		/// </summary>
		public static readonly Guid DefaultTemplateMerchantGuid = Guid.Parse("99999999-9999-9999-9999-999999999999");

        /// <summary>
        /// 登陆信息存放在token中的Key
        /// </summary>
        public static readonly string LoginInfoKey = "logininfo";

        /// <summary>
        /// 默认区域id，为中国
        /// </summary>
        public static Guid DefaultReginId = Guid.Parse("00000000-0000-0000-0000-000000010001");

    }
}
