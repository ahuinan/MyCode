using MyCode.Project.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message;
using MyCode.Project.Domain.Model;
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Search;
using MyCode.Project.Domain.Message.Response.Jurisdiction;

namespace MyCode.Project.Repositories
{
    public class SysRoleMenuRepository: Repository<SysRoleMenu>, ISysRoleMenuRepository
    {
        public SysRoleMenuRepository(MyCodeSqlSugarClient context) : base(context)
        { }

        #region GetMenuList(根据角色ID得到菜单)
        public List<MenuList> GetMenuList(Guid roleId)
        {
            var strSql = $@"SELECT 
	SM.MenuId,
	SM.Name,
	SM.SystemType,
	SM.ParentID,
	(CASE WHEN RM.RoleMenuID IS NULL THEN 0 ELSE 1 End) IsChecked,
    SM.OrderNo
FROM
	SysMenu SM WITH (nolock)
LEFT JOIN
	dbo.SysRoleMenu RM WITH (nolock) ON RM.MenuID = SM.MenuID
WHERE RM.RoleID = '{roleId}'";

            return this.SelectList<MenuList>(strSql);
        }
        #endregion

        

    }
}