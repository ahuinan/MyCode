using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Config;
using MyCode.Project.Domain.Message;
using MyCode.Project.Domain.Message.Response.Delivery;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.WebPost;

namespace MyCode.Project.OutSideService.Implementation
{
	public class DeliverySheetService : IDeliverySheetService
	{

		public DeliverySheetService()
		{
		
		}


		#region GetDeliverySheet(获取菜鸟面单号，传入JSON字符串)
		public CaiNiaoDeliveryResp GetDeliverySheet(string interfaceParams, string data)
		{
			WebUtils webUtils = new WebUtils();
			Dictionary<string, string> parms = new Dictionary<string, string>();
			parms.Add("appKey", "GetDeliverySheet");
			parms.Add("appSecret", "GetDeliverySheet");
			parms.Add("type", "1");
			parms.Add("interfaceParams", interfaceParams);
			parms.Add("data", data);

			var result = JObject.Parse(webUtils.DoPost(SystemConfig.DeliveryUrl, parms));

			if (result != null && result.HasValues && result["code"].ToString() == "0")
			{
				return new CaiNiaoDeliveryResp()
				{
					WaybillCode = result["data"].First.Value<string>("waybillCode"),
					ZoneCode = result["data"].First.Value<string>("shortAddress"),
					AddressId = result["data"].First.Value<string>("packageCenterCode"),
					PackageCenterName = result["data"].First.Value<string>("packageCenterName")
				} ;
			}
			throw new BaseException(result["message"].ToString());
		}
		#endregion

	}

}
