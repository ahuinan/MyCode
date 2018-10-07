using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.WebApi.Controllers;
using MyCode.Project.Domain.Message.Response.User;
using MyCode.Project.Infrastructure.Constant;
using MyCode.Project.Domain.Message.Request.User;
using MyCode.Project.Domain.Message.Act.User;
using MyCode.Project.Services;
using MyCode.Project.Infrastructure.Extensions;
using MyCode.Project.Domain.Message.Act.Common;
using MyCode.Project.Domain.Message.Response.Jurisdiction;

namespace MyCode.Project.WebApi.Areas.Admin.Controllers
{
    public class JurisdictionController : BaseAdminController
    {
        private readonly IJurisdictionService _jurisdictionService;

        public JurisdictionController(IJurisdictionService jurisdictionService)
        {
            _jurisdictionService = jurisdictionService;
        }

        /// <summary>
        /// 添加或者修改账户
        /// </summary>
        /// <param name="request">请求参数</param>
        [HttpPost]
        public void SaveLogin(SaveLoginAct request)
        {
            var curr = CurrentLogin;

            if (request.LoginId.SafeValue() == Guid.Empty)
            {
                _jurisdictionService.AddLogin(request, curr.UserName, Guid.Empty);
            }
            else
            {
                _jurisdictionService.UpdateLogin(request, curr.UserName);
            }
        }

        /// <summary>
        /// 根据loginid得到账户详情
        /// </summary>
        /// <param name="id">loginId</param>
        /// <returns></returns>
        [HttpGet]
        public LoginDetailResp GetLoginDetail(Guid id)
        {
            return _jurisdictionService.GetLoginDetail(id);
        }

        /// <summary>
        /// 修改账户启用禁用状态
        /// </summary>
        /// <param name="act"></param>
        [HttpPost]
        public void ChangeLoginStatus(ChangeStatusAct act)
        {
            _jurisdictionService.ChangeLoginStatus(act, CurrentLogin.UserName);
        }

        /// <summary>
        /// 得到账户分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public PageResult<LoginListResp> GetLoginPageList(PagedSearch request)
        {
            return _jurisdictionService.GetLoginPageList(request, CurrentLogin.MerchantId);
        }

        /// <summary>
        /// 得到角色信息包括菜单情况
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpGet]
        public RoleMenu GetRoleInfo(Guid id)
        {
            return _jurisdictionService.GetRoleInfo(id);
        }

        /// <summary>
        /// 得到角色分页列表
        /// </summary>
        /// <param name="request">请求参数</param>
        [HttpPost]
        public PageResult<RoleListResp> GetRolePageList(PagedSearch request)
        {
            return _jurisdictionService.GetRolePageList(request, CurrentLogin.MerchantId);
        }

        /// <summary>
        /// 根据系统类型返回菜单数据
        /// </summary>
        /// <param name="systemtype">系统类型</param>
        /// <returns></returns>
        [HttpGet]
        public List<MenuList> GetMenuList(int systemtype)
        {
            return _jurisdictionService.GetMenuList(systemtype, CurrentLogin.UserId);
        }
    }
}
