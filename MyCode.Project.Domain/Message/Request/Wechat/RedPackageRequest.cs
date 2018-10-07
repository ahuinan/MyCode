using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Request.Wechat
{
    public class RedPackageRequest
    {
        /// <summary>
		/// 微信公众号KEY
		/// </summary>
		public string AppId
        {
            get;
            set;
        }

        /// <summary>
        /// 商户号
        /// </summary>
        public string MchId
        {
            get;
            set;
        }

        /// <summary>
        /// 商户订单号
        /// </summary>
        public string BillNo
        {
            get;
            set;
        }

        /// <summary>
        /// 支付密码
        /// </summary>
        public string PayKey
        {
            get;
            set;
        }

        /// <summary>
        /// 证书路径
        /// </summary>
        public string CertificatePath
        {
            get;
            set;
        }

        /// <summary>
        /// 证书密码
        /// </summary>
        public string CertificatePwd
        {
            get;
            set;
        }

        /// <summary>
        /// 红包发送者名称-微钉
        /// </summary>
        public string SendFrom
        {
            get;
            set;
        }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string UserHostIp
        {
            get;
            set;
        }

        /// <summary>
        /// 发多少钱，单位元
        /// </summary>
        public decimal Money
        {
            get; set;
        }

        /// <summary>
        /// 祝福语
        /// </summary>
        public string RedPackageWish
        {
            get;
            set;
        }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActivityName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            get;
            set;
        }

        /// <summary>
        /// 发给谁
        /// </summary>
        public string OpenId
        {
            get;
            set;
        }
    }
}
