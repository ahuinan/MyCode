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
using MyCode.Project.Domain.Message.Response.User;

namespace MyCode.Project.Domain.Repositories
{
	public interface ISysLoginRepository : IRepository<SysLogin>
	{
        /// <summary>
        /// 得到账户分页列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="merchantId">商家ID</param>
        /// <returns></returns>
        PageResult<LoginListResp> GetLoginPageList(PagedSearch request, Guid merchantId);

    }
}
