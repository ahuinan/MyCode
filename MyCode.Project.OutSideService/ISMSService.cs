using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.OutSideService
{
	public interface ISMSService
	{
		/// <summary>
		/// type:消息类型 1:验证码 2:找回密码
		/// mobile:要发送的手机号码
		/// verifyCode:要发送的验证码
		/// </summary>
		void SendMessage(string type, string mobile, string verifyCode);

	}
}
