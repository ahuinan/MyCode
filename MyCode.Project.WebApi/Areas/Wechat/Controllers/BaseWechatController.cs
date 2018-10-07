using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyCode.Project.Domain.Message.Response.User;
using MyCode.Project.Infrastructure.Constant;
using MyCode.Project.WebApi.Controllers;

namespace MyCode.Project.WebApi.Areas.Wechat.Controllers
{
    public class BaseWechatController : BaseAPIController
    {
        /// <summary>
        /// 取得登陆信息
        /// </summary>
        protected LoginInfo CurrentLogin
        {
            get
            {
                var obj = this.RequestContext.RouteData.Values[Const.LoginInfoKey];

                return ((JObject)obj).ToObject<MemberLoginInfo>();
            }
        }
    }
}
