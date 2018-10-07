using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Services;
using MyCode.Project.Domain.Message;
using AutoMapper;
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Domain.Model;
using MyCode.Project.Infrastructure.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Transactions;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Enumeration;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.OutSideService;
using MyCode.Project.Domain.Config;
using System.Reflection;
using MyCode.Project.Repositories.Common;
using MyCode.Project.Domain.Message.Response.User;

namespace MyCode.Project.Services.Implementation
{
    public class UserService: ServiceBase,IUserService
    {
        private readonly IWorkProcessRepository _workProcessRepository;

        public UserService(IWorkProcessRepository workProcessRepository)
        {
            _workProcessRepository = workProcessRepository;
        }

        [TransactionCallHandler]
        public void AddUser()
        {

        }


        #region LoginInfo(根据账号和密码登陆)
        public LoginInfo AdminLogin(string account, string password)
        {
            return new AdminLoginInfo()
            {
                UserId = Guid.Empty,
                UserName = "测试管理账号"
            };
        }
        #endregion

        #region MemberLogin(根据授权key来登陆,key有缓存时间)
        public LoginInfo MemberLogin(string key)
        {
            return new MemberLoginInfo()
            {
                UserId = Guid.Empty,
                UserName = "测试用户账号"
            };
        }
        #endregion

    }
}
