using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Jurisdiction
{
    public class MenuList
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public int IsChecked { get; set; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 系统类型
        /// </summary>
        public int SystemType { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<MenuList> SubMenus { get; set; }
    }
}
