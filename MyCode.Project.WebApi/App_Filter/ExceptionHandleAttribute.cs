using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MyCode.Project.WebApi.App_Filter
{
    public class ExceptionHandleAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
			//参考：ResultHandleAttribute
			#region
			base.OnException(actionExecutedContext);
			var errorMessage = actionExecutedContext.Exception.Message;

			var result = new Result();
			result.ResultCode = ResultCode.Error;
			if (actionExecutedContext.Exception is BaseException )
			{
				result.ErrorMessage = errorMessage;
			}
			else
			{
				result.ErrorMessage = "网络繁忙，请勿频繁点击";
			}

			actionExecutedContext.Response = actionExecutedContext.Request
				.CreateResponse(System.Net.HttpStatusCode.OK, result, "application/json");
			#endregion

		}




    }



}