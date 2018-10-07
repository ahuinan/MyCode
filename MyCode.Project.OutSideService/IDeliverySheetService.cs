using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message.Response.Delivery;

namespace MyCode.Project.OutSideService
{
	public interface IDeliverySheetService
	{
        /// <summary>
        /// 获取快递面单号
        /// </summary>
        /// <returns></returns>
        CaiNiaoDeliveryResp GetDeliverySheet(string interfaceParams,string data);
	}
}
