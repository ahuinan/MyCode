using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.Common;
using System.IO;

namespace MyCode.Project.WebApi.App_Filter
{
    public class ResultHandleAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
                        
			if (actionExecutedContext.Exception != null)
			{
                var stream = actionExecutedContext.Request.Content.ReadAsStreamAsync().Result;
                string requestDataStr = "";
                if (stream != null && stream.Length > 0)
                {
                    stream.Position = 0; //当你读取完之后必须把stream的读取位置设为开始
                    using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        requestDataStr = reader.ReadToEnd().ToString();
                    }
                }


                string ErrorMsg = Environment.NewLine + "-----------------------------------------" + Environment.NewLine;
                LogHelper.Error(ErrorMsg,null);
                LogHelper.Error(actionExecutedContext.Request.RequestUri.ToString() + Environment.NewLine,null);
                LogHelper.Error("请求参数：" + requestDataStr + Environment.NewLine,null);
                LogHelper.Error("Error:", actionExecutedContext.Exception);

                throw actionExecutedContext.Exception;
			}            

            var actionContext = actionExecutedContext.ActionContext;

            //忽略结果过滤
            var ignore = actionContext.ActionDescriptor.GetCustomAttributes<IgnoreResultHandleAttribute>().Any() ||
                         actionContext.ControllerContext.ControllerDescriptor
                             .GetCustomAttributes<IgnoreResultHandleAttribute>().Any();
            if (!ignore)
            {
                var response = actionContext.Response;
                Result result = new Result();
                result.ResultCode = ResultCode.Success;
                if (response != null && response.Content != null)
                {
                    result.Data = response.Content.ReadAsAsync<object>().Result;
                }

                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, result, "application/json");
            }
            else
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
        }

    }
}