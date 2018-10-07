using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Security;
using MyCode.Project.Domain.Config;
using MyCode.Project.Domain.Message.Response.User;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Constant;

namespace MyCode.Project.WebApi.App_Filter
{
    public class AuthorizeFilter: AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext filterContext)
        {
		    var attr = filterContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();

		    bool isAnonymous = attr.Any(p => p is AllowAnonymousAttribute);

		    if (isAnonymous) { return; }

            var authHeader = from h in filterContext.Request.Headers where h.Key == "Authorization" select h.Value.FirstOrDefault();

            Result r = new Result();

            bool successLogin = false;

            string errMsg = "";

            if (authHeader != null)
            {
                try
                {
                    var logininfo = TokenHelper.Get(authHeader.FirstOrDefault().Trim(),
                        SystemConfig.JwtKey,
                        Const.LoginInfoKey);

                    if (logininfo != null)
                    {
                        filterContext.ControllerContext.RouteData.Values[Const.LoginInfoKey] = logininfo;
                    }

                    successLogin = true;

                }
                catch (Exception ex)
                {
                    r.ErrorMessage = ex.Message;
                }
            }
            else {

                r.ErrorMessage = "请重新登录";
            }

            if (!successLogin)
            {
                r.ResultCode = ResultCode.Error;
                filterContext.Response = filterContext.Request.CreateResponse<Result>(HttpStatusCode.OK, r, "application/json");
                return;
            }

        }


    }
}
