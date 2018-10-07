using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Business.WeiXin
{
    /// <summary>
    /// 微信下单支付返回值
    /// </summary>
    public class WxPayReturn
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        public WxPayState ReturnCode { get; set; }

        /// <summary>
        /// 微信生成的预支付ID，用于后续接口调用中使用
        /// </summary>
        public string PrepayId { get; set; }

        /// <summary>
        /// 微信支付分配的终端设备号 
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// trade_type为NATIVE时有返回，此参数可直接生成二维码展示出来进行扫码支付
        /// </summary>
        public string CodeUrl { get; set; }
    }
}
