using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Business.WeiXin
{
    /// <summary>
    /// 微信支付回调参数
    /// </summary>
    public class WxPayReceive
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 商品单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商家数据包，附加信息
        /// </summary>
        public string Attach { get; set; }

        /// <summary>
        /// 微信用户openid
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 支付完成时间
        /// </summary>
        public DateTime TimeEnd { get; set; }

		/// <summary>
		/// 订单金额，单位元
		/// </summary>
		public decimal TotalFee { get; set; }


    }
}
