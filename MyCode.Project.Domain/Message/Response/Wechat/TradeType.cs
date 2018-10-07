using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Business.WeiXin
{
    /// <summary>
    /// 交易类型
    /// </summary>
    public enum TradeType
    {
        /// <summary>
        /// 公众号支付
        /// </summary>
        [Description("JSAPI")]
        JsApi=0,
        /// <summary>
        /// 原生扫码支付
        /// </summary>
        [Description("NATIVE")]
        Native =1,
        /// <summary>
        /// App支付
        /// </summary>
        [Description("APP")]
        App=2
    }
}
