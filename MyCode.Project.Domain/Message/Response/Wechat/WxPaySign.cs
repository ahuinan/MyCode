using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Business.WeiXin
{
    /// <summary>
    /// 微信支付参数
    /// </summary>
    public class WxPaySign
    {
        /// <summary>
        /// 签名类型，默认MD5，支持HMAC-SHA256和MD5
        /// </summary>
        public string SignType { get; set; }

        /// <summary>
        /// 随机字符串
        /// </summary>
        public string NonceStr { get;set; }

        /// <summary>
        /// 预支付ID
        /// </summary>
        public string Package { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; set; }

        /// <summary>
        /// 支付签名
        /// </summary>
        public string PaySign { get; set; }

		/// <summary>
		///  AppId
		/// </summary>
		public string AppId { get; set; }
    }
}
