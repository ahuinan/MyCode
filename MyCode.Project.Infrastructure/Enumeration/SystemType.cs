using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Enumeration
{
	public enum SystemType
	{
		/// <summary>
		/// ERP系统
		/// </summary>
		[Description("ERP系统")]
		ERP = 100,

        [Description("书店系统")]
        Book = 200
	}
}
