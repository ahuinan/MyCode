using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Request.Wechat
{
    public class GetRedPackageRequest
    {
        /// <summary>
		/// 商户号
		/// </summary>
		public string MchId
        {
            get;
            set;
        }

        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string AppId
        {
            get;
            set;
        }

        /// <summary>
        /// 商家订单号
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
    }
}
