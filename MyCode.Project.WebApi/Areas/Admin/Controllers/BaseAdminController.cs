using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyCode.Project.WebApi.Controllers;
using MyCode.Project.Domain.Message.Response.User;
using Newtonsoft.Json;
using System.Threading;
using MyCode.Project.Infrastructure.Constant;
using Newtonsoft.Json.Linq;

namespace MyCode.Project.WebApi.Areas.Admin.Controllers
{
    public class BaseAdminController : BaseAPIController
    {
        /// <summary>
        /// 取得登陆信息
        /// </summary>
        protected AdminLoginInfo CurrentLogin
        {
            get
            { 
                var obj = this.RequestContext.RouteData.Values[Const.LoginInfoKey];

                return ((JObject)obj).ToObject<AdminLoginInfo>();
            }
        }
    }
}
