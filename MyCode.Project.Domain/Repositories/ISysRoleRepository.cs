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
	public interface ISysRoleRepository : IRepository<SysRole>
	{
        /// <summary>
        /// 得到角色分页列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="merchantId">商家ID</param>
        /// <returns></returns>
        PageResult<RoleListResp> GetRolePageList(PagedSearch request, Guid merchantId);

    }
}
