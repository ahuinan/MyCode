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
using MyCode.Project.Domain.Model;
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Enumeration;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.Extensions;
using MyCode.Project.Infrastructure.UnityExtensions;
using MyCode.Project.Repositories.Common;

namespace MyCode.Project.Services.Implementation
{
    /// <summary>
    /// 权限管理模块
    /// </summary>
    public class JurisdictionService: IJurisdictionService
    {
        #region 初始化
        private readonly ISysLoginRepository _sysLoginRepository;
        private readonly ISysLoginRoleRepository _sysLoginRoleRepository;
        private readonly ISysRoleMenuRepository _sysRoleMenuRepository;
        private readonly ISysRoleRepository _sysRoleRepository;

        public JurisdictionService(ISysLoginRepository sysLoginRepository,
            ISysLoginRoleRepository sysLoginRoleRepository,
            ISysRoleMenuRepository sysRoleMenuRepository,
            ISysRoleRepository sysRoleRepository)
        {
            _sysLoginRepository = sysLoginRepository;
            _sysLoginRoleRepository = sysLoginRoleRepository;
            _sysRoleMenuRepository = sysRoleMenuRepository;
            _sysRoleRepository = sysRoleRepository;
        }
        #endregion

        #region GetLoginInfo(根据账户和密码得到管理员信息)
        public AdminLoginInfo GetLoginInfo(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Account)) { throw new BaseException("账户不能为空"); }

            if (string.IsNullOrEmpty(request.Password)) { throw new BaseException("密码不能为空"); }

            var login = _sysLoginRepository.Queryable().Where(p => p.Login == request.Account && p.Password == EncryptHelper.SHA1Hash(request.Password)).Select(p => new { p.Login, p.LoginID,p.Status,p.MerchantID }).First();

            if (login == null) { throw new BaseException("账号或者密码错误"); }

            if (login.Status == (int)Status.Disable) { throw new BaseException("该账户已被禁用"); }

            return new AdminLoginInfo()
            {
                UserId = login.LoginID,
                UserName = login.Login,
                MerchantId = login.MerchantID
            };
        }
        #endregion

        #region AddLogin(添加一个登陆账户)
        [TransactionCallHandler]
        public void AddLogin(SaveLoginAct request,string editor,Guid merchantId)
        {
            if (string.IsNullOrEmpty(request.Login)){ throw new BaseException("账号不能为空！");}

            if (string.IsNullOrEmpty(request.Password)){throw new BaseException("密码不能为空！");}

            var count = _sysLoginRepository.Count(e => e.Login == request.Login && e.MerchantID == merchantId);

            if (count > 0){ throw new BaseException("账号已存在！"); }

            SysLogin login = new SysLogin()
            {
                LoginID = Guid.NewGuid(),
                MerchantID = merchantId,
                Login = request.Login,
                Name = request.Name,
                Status = (int)Status.Enable,
                Password = EncryptHelper.SHA1Hash(request.Password),
                Creater = editor,
                Editor = editor,
                CreateTime = DateTime.Now,
                EditTime = DateTime.Now,
                Tele = request.Tele
            };
            _sysLoginRepository.Add(login);

            if (request.RoleId.SafeValue() != Guid.Empty)
            {
                SysLoginRole loginRole = new SysLoginRole()
                {
                    LoginID = login.LoginID,
                    LoginRoleID = Guid.NewGuid(),
                    MerchantID = merchantId,
                    RoleID = request.RoleId.SafeValue()
                };
                _sysLoginRoleRepository.Add(loginRole);
            }
        }
        #endregion

        #region UpdateLogin(修改一个登陆账户)
        [TransactionCallHandler]
        public void UpdateLogin(SaveLoginAct request, string editor)
        {
            var login = _sysLoginRepository.SelectFirst(p => p.LoginID == request.LoginId);

            if (!string.IsNullOrEmpty(request.Password)) { login.Password = EncryptHelper.SHA1Hash(request.Password); }

            login.Editor = editor;
            login.EditTime = DateTime.Now;
            login.Name = request.Name;
            login.Tele = request.Tele;

            _sysLoginRepository.Update(login);

            //先删掉，再加
            _sysLoginRoleRepository.Delete(p => p.LoginID == request.LoginId);

            if (request.RoleId.SafeValue() != Guid.Empty)
            {
                SysLoginRole loginRole = new SysLoginRole()
                {
                    LoginID = login.LoginID,
                    LoginRoleID = Guid.NewGuid(),
                    MerchantID = login.MerchantID,
                    RoleID = request.RoleId.SafeValue()
                };
                _sysLoginRoleRepository.Add(loginRole);
            }
        }
        #endregion

        #region GetLoginDetail(根据loginId得到详情)
        public LoginDetailResp GetLoginDetail(Guid loginId)
        {
            var login = _sysLoginRepository.SelectFirst(p => p.LoginID == loginId);

            var loginResp =  new LoginDetailResp()
            {
                LoginId = loginId,
                Login = login.Login,
                Name = login.Name,
                Tele = login.Tele
            };

            var loginRole = _sysLoginRoleRepository.Queryable().Where(p => p.LoginID == loginId).Select(p => new { p.RoleID }).First();

            if (loginRole != null) { loginResp.RoleId = loginRole.RoleID; }

            return loginResp;
        }
        #endregion

        #region ChangeLoginStatus(修改启用禁用状态)
        [TransactionCallHandler]
        public void ChangeLoginStatus(ChangeStatusAct act, string editor)
        {
            act.CheckStatus();

            var logins = act.ListId.Select(line => new SysLogin { LoginID = line, Status = act.Status, Editor = editor, EditTime = DateTime.Now });

            _sysLoginRepository.Update(logins, it => new { it.Status, it.Editor, it.EditTime });
        }
        #endregion

        #region GetLoginPageList(得到账户分页列表)
        public PageResult<LoginListResp> GetLoginPageList(PagedSearch request, Guid merchantId)
        {
            return _sysLoginRepository.GetLoginPageList(request, merchantId);
        }
        #endregion

        #region GetRoleInfo(得到角色信息，包括该角色拥有的菜单)
        public RoleMenu GetRoleInfo(Guid roleId)
        {
            var role = _sysRoleRepository.Queryable().Where(p => p.RoleID == roleId).Select(p => new { p.RoleID, p.Name }).First();

            var roleMenu = new RoleMenu()
            {
                Name = role.Name,
                RoleId = roleId,
                Menus = new List<MenuResp>()
            };
            var menuList = _sysRoleMenuRepository.GetMenuList(roleId);

            if (menuList == null || menuList.Count == 0) { return roleMenu; }

            var systemTypes = menuList.Select(p => p.SystemType).Distinct();

            foreach (var systemType in systemTypes)
            {
                var menuResp = new MenuResp();

                menuResp.SystemType = systemType;

                var menus = new List<MenuList>();

                var rootMenus = menuList.Where(p => p.ParentId == null && p.SystemType == systemType).OrderBy(p => p.OrderNo).ToList();

                foreach (var menu in rootMenus)
                {
                    menus.Add(menu);
                }

                foreach (var item in menus)
                {
                    var subMenus = menuList.Where(p => p.ParentId == item.MenuId).OrderBy(p => p.OrderNo).ToList();

                    //这里只计算两级菜单了
                    item.SubMenus = subMenus;
                }

                menuResp.MenuList = menus;

                roleMenu.Menus.Add(menuResp);
            }

            return roleMenu;

        }
        #endregion

        #region GetRolePageList(得到角色分页列表)
        public PageResult<RoleListResp> GetRolePageList(PagedSearch request, Guid merchantId)
        {
            return _sysRoleRepository.GetRolePageList(request, merchantId);
        }
        #endregion

        #region GetMenuList(根据系统ID得到有效的要显示的菜单)
        public List<MenuList> GetMenuList(int systemType,Guid loginId)
        {
            var role = _sysLoginRoleRepository.Queryable().Where(p => p.LoginID == loginId).Select(p => new { p.RoleID }).First();

            var menuList = _sysRoleMenuRepository.GetMenuList(role.RoleID);

            if (menuList == null || menuList.Count == 0) { return new List<MenuList>(); }

            var menus = new List<MenuList>();

            var rootMenus = menuList.Where(p => p.ParentId == null && p.SystemType == systemType && p.IsChecked == 1).OrderBy(p => p.OrderNo).ToList();

            foreach (var menu in rootMenus)
            {
                menus.Add(menu);
            }

            foreach (var item in menus)
            {
                var subMenus = menuList.Where(p => p.ParentId == item.MenuId).OrderBy(p => p.OrderNo).ToList();

                //这里只计算两级菜单了
                item.SubMenus = subMenus;
            }

            return menus;
        }
        #endregion
    }
}
