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
using MyCode.Project.Services;
using MyCode.Project.Domain.Config;

namespace MyCode.Project.WebApi.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly IJurisdictionService _jurisdictionService;

        public UserController(IJurisdictionService jurisdictionService)
        {
            _jurisdictionService = jurisdictionService;
        }

        /// <summary>
        /// 测试日志
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public void TestLog()
        {
            LogHelper.Error("错误测试str");
            LogHelper.Error("错误测试Exception", new Exception("ERROR") { });
        }

        [HttpGet]
        public LoginInfo CurrentAdmin()
        { 
            var userid =  CurrentLogin.UserId;

            var username = CurrentLogin.UserName;

            return CurrentLogin;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        public string Login(LoginRequest request)
        {
            var adminInfo =  _jurisdictionService.GetLoginInfo(request);

            return TokenHelper.CreateToken(SystemConfig.JwtKey,
                Const.LoginInfoKey,
                adminInfo);
        }
    }
}
