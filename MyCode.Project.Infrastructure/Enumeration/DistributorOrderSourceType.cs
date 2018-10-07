using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Project.Infrastructure.Enumeration
{
	public enum DistributorOrderSourceType
    {
		/// <summary>
		/// 没有来源
		/// </summary>
		[Description("没有来源")]
		Normal = 0,

		/// <summary>
		/// 调整单
		/// </summary>
		[Description("调整单")]
		ExchangeOrder = 1,

		/// <summary>
		/// 云店订单
		/// </summary>
		[Description("云店订单")]
		CloudOrder = 2


	}
}
