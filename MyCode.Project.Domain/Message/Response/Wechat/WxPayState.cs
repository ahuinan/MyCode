using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Business.WeiXin
{
    /// <summary>
    /// 微信支付返回状态
    /// </summary>
    public enum WxPayState
    {
        /// <summary>
        /// 成功生成Prepay_id
        /// </summary>
        Success,
        /// <summary>
        /// 已支付
        /// </summary>
        OrderPayid,
        /// <summary>
        /// out_trade_no已使用， 返回 prepay_id， 继续完成支付
        /// </summary>
        OutTradeNoUsed,
        /// <summary>
        /// 支付失败
        /// </summary>
        Fail
    }
}
