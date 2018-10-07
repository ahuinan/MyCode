using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Jurisdiction
{
    public class MenuResp
    {
        /// <summary>
		/// 系统类型 
		/// </summary>
		public int SystemType
        {
            get;
            set;
        }

        /// <summary>
        /// 菜单
        /// </summary>
        public List<MenuList> MenuList
        {
            get;
            set;
        }
    }
}
