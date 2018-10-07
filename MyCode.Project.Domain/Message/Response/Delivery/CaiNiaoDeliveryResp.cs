using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Delivery
{
    public class CaiNiaoDeliveryResp
    {
        /// <summary>
		/// 运单号
		/// </summary>
		public string WaybillCode { get; set; }

        /// <summary>
        /// 大头笔
        /// </summary>
        public string ZoneCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AddressId { get; set; }

        /// <summary>
        /// 集包地
        /// </summary>
        public string PackageCenterName { get; set; }
    }
}
