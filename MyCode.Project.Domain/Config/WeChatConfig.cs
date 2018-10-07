using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Config
{
    public class WeChatConfig
    {
		public long TokenOverTime { get; set; }
		public long TiketOverTime { get; set; }
		public string Token { get; set; }
		public string Tiket { get; set; }
		public long TokenCreateTime { get; set; }
		public long TiketCreateTime { get; set; }

        /// <summary>
        /// AppID(应用ID)
        /// </summary>
		public string Appkey { get; set; }

        /// <summary>
        /// AppSecret(应用密匙)
        /// </summary>
		public string AppSecret { get; set; }
		public Guid InfOauthDetailID { get; set; }

        /// <summary>
        /// PartnerKey(初始支付密匙)
        /// </summary>
		public string Key { get; set; }

        /// <summary>
        /// MchID(支付商户号)
        /// </summary>
		public string MchID { get; set; }
		public Guid MerchantID { get; set; }

		/// <summary>
		/// 证书路径
		/// </summary>
		public string CertificatePath { get; set; }

		/// <summary>
		/// 证书密码
		/// </summary>
		public string CertificatePwd { get; set; }

		//public string APPID
		//{
		//    get;
		//    set;
		//}

		//public string MCHID
		//{
		//    get;
		//    set;
		//}

		//public string KEY
		//{
		//    get;
		//    set;
		//}

		//public string APPSECRET
		//{
		//    get;
		//    set;
		//}

		//public string IP
		//{
		//    get;
		//    set;
		//}

		//public string NOTIFY_URL
		//{
		//    get;
		//    set;
		//}

	}
}
