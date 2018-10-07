using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.WebApi.Controllers;

namespace MyCode.Project.WebApi.Areas.Wechat.Controllers
{
    public class UserController : BaseWechatController
    {
        public UserController()
        {

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
    }
}
