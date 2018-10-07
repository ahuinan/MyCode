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
    public class SysRoleRepository: Repository<SysRole>, ISysRoleRepository
    {
        public SysRoleRepository(MyCodeSqlSugarClient context) : base(context)
        { }


        #region GetRolePageList(得到角色分页列表)
        public PageResult<RoleListResp> GetRolePageList(PagedSearch request, Guid merchantId)
        {
            var strSql = $@"SELECT 
	RoleId,
	Name, 
	Editor,
	EditTime,
	Status
FROM sysrole WITH (nolock)
WHERE merchantid = '{merchantId}'";

            return this.SelectListPage<RoleListResp>(strSql, request.Page, request.PageSize, "EditTime", true);
        }
        #endregion



    }
}