using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure;
using MyCode.Project.Domain;
using MyCode.Project.Domain.Model;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Domain.Message;
using MyCode.Project.Domain.Message.Response.Jurisdiction;

namespace MyCode.Project.Domain.Repositories
{
	public interface ISysRoleMenuRepository : IRepository<SysRoleMenu>
	{
        /// <summary>
        /// 根据角色ID得到菜单
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        List<MenuList> GetMenuList(Guid roleId);

    }
}
