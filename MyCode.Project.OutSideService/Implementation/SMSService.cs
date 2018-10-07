using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Config;
using MyCode.Project.Infrastructure.WebPost;
using Newtonsoft.Json;
using MyCode.Project.Domain.Message;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Domain.Message.Response.Common;

namespace MyCode.Project.OutSideService.Implementation
{
	public class SMSService:ISMSService
	{

		public SMSService() {

		}

		public void SendMessage(string type, string mobile, string verifyCode) {
			WebUtils webUtils = new WebUtils();
			var dic = new Dictionary<string, string>();
			dic["type"] = type;
			dic["mobile"] = mobile;
			dic["verifyCode"] = verifyCode;

			SMSResponse result = JsonConvert.DeserializeObject<SMSResponse>(webUtils.DoPost(SystemConfig.SmsServiceUrl, dic));
			if (result.code != "0") {
				throw new BaseException(result.cause);
			}
		}
	}
}
