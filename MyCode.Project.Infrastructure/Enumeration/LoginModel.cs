using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Enumeration
{
	public enum LoginModelEnum
	{ 
		[Description("普通登录")]
		Normal = 0,

		/// <summary>
		/// 停用
		/// </summary>
		[Description("手势登录")]
		HandPassword = 1
	}
}
