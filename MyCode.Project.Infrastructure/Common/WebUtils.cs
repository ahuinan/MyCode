using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Wolf.Project.Infrastructure.Common
{
	public class WebUtils
	{
		/// <summary>
		/// 取得来源URL
		/// </summary>
		/// <returns></returns>
		public static string GetSourceUrl() {
			return HttpContext.Current.Request.UrlReferrer.Host.ToString();
		}
	}
}
