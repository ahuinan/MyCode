using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message.Act.Common;
using MyCode.Project.Domain.Message.Act.User;
using MyCode.Project.Domain.Message.Request.User;
using MyCode.Project.Domain.Message.Response.Jurisdiction;
using MyCode.Project.Domain.Message.Response.User;
using MyCode.Project.Infrastructure.Common;

namespace MyCode.Project.Services
{
    public interface IJurisdictionService
    {

        /// <summary>
        /// 根据账户和密码得到管理员信息
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns></returns>
        AdminLoginInfo GetLoginInfo(LoginRequest request);

        /// <summary>
        /// 添加一个登陆账户
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="editor">操作人</param>
        /// <param name="merchantId">商家ID</param>
        void AddLogin(SaveLoginAct request, string editor, Guid merchantId);

        /// <summary>
        /// 根据loginId得到详情
        /// </summary>
        /// <param name="loginId">主键ID</param>
        /// <returns></returns>
        LoginDetailResp GetLoginDetail(Guid loginId);

        /// <summary>
        /// 修改一个登陆账户
        /// </summary>
        /// <param name="request">请求实体</param>
        /// <param name="editor">修改人</param>
        void UpdateLogin(SaveLoginAct request, string editor);

        /// <summary>
        /// 修改启用禁用状态
        /// </summary>
        /// <param name="act"></param>
        /// <param name="editor"></param>
        void ChangeLoginStatus(ChangeStatusAct act, string editor);

        /// <summary>
        /// 得到账户分页列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="merchantId">商家ID</param>
        /// <returns></returns>
        PageResult<LoginListResp> GetLoginPageList(PagedSearch request, Guid merchantId);

        /// <summary>
        /// 得到角色详情，包括拥有的菜单信息；
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RoleMenu GetRoleInfo(Guid roleId);

        /// <summary>
        /// 得到角色分页列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="merchantId">商家ID</param>
        /// <returns></returns>
        PageResult<RoleListResp> GetRolePageList(PagedSearch request, Guid merchantId);

        /// <summary>
        /// 根据系统类型和登陆ID得到有效的菜单数据
        /// </summary>
        /// <param name="systemType">系统类型</param>
        /// <param name="loginId">登陆id</param>
        /// <returns></returns>
        List<MenuList> GetMenuList(int systemType, Guid loginId);


    }
}
