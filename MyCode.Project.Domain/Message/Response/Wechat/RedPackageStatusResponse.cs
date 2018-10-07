using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Wechat
{
    public class RedPackageStatusResponse
    {
        /// <summary>
		/// 返回状态码，SUCCESS/FAIL
		/// </summary>
		public string return_code
        {
            get;
            set;
        }

        /// <summary>
        /// 返回信息：如非空，为错误原因
        /// </summary>
        public string return_msg
        {
            get;
            set;
        }

        /// <summary>
        /// return_code为SUCCESS的时候有返回，SUCCESS/FAIL，以下字段在return_code为SUCCESS的时候有返回
        /// </summary>
        public string result_code
        {
            get;
            set;
        }

        /// <summary>
        /// 错误码信息
        /// </summary>
        public string err_code
        {
            get;
            set;
        }

        /// <summary>
        /// 结果信息描述
        /// </summary>
        public string err_code_des
        {
            get;
            set;
        }

        /// <summary>
        /// 商户订单号，以下字段在return_code 和result_code都为SUCCESS的时候有返回
        /// </summary>
        public string mch_billno
        {
            get;
            set;
        }

        /// <summary>
        /// 红包单号
        /// </summary>
        public string detail_id
        {
            get;
            set;
        }

        /// <summary>
        /// 红包状态,SENDING:发放中  SENT:已发放待领取 FAILED：发放失败 RECEIVED:已领取 RFUND_ING:退款中 REFUND:已退款
        /// </summary>
        public string status
        {
            get;
            set;
        }

        /// <summary>
        /// 红包金额，单位分
        /// </summary>
        public int total_amount
        {
            get;
            set;
        }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string reason
        {
            get;
            set;
        }

        /// <summary>
        /// 领取红包的openid
        /// </summary>
        public string openid
        {
            get;
            set;
        }
    }
}
