using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Enumeration
{

	/// <summary>
	/// 图片类型
	/// </summary>
	public enum ImageType
	{
		/// <summary>
		/// 商品
		/// </summary>
		[Description("商品")]
		Goods = 11,

		
		/// <summary>
		/// 头像
		/// </summary>
		[Description("头像")]
		Avatar = 21

		
	}
}
