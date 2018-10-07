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
    public class WxPayParam
    {
        /// <summary>
        /// 微信应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 微信支付——商户ID
        /// </summary>
        public string MchId { get; set; }

        /// <summary>
        /// 微信支付——设备号
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 微信支付——服务端IP地址
        /// </summary>
        public string ServiceIpAddress { get; set; }

        /// <summary>
        /// 微信支付——商户支付密匙
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 交易类型
        /// </summary>
        public TradeType TradeType { get; set; }

        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 商品名称明细列表
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 附加参数。
        /// 在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据
        /// </summary>
        public string Attach { get; set; }

        /// <summary>
        /// 商品单ID
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 结算货币，默认人民币：CNY
        /// </summary>
        public string FeeType { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal TotalFee { get; set; }

        /// <summary>
        /// 回调Url，支付成功或失败回调的Url
        /// </summary>
        public string NotifyUrl { get; set; }

        /// <summary>
        /// 微信用户标识，trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。
        /// </summary>
        public string Openid { get; set; }
    }
}
