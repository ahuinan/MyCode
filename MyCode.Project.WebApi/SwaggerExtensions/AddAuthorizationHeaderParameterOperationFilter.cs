using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Filters;
using Swashbuckle.Swagger;

namespace MyCode.Project.WebApi.SwaggerExtensions
{
    /// <summary>
    /// 添加授权认证请求头参数操作过滤
    /// </summary>
    public class AddAuthorizationHeaderParameterOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }
            var filtetPipeline = apiDescription.ActionDescriptor.GetFilterPipeline();

            //判断是否添加权限过滤器
            var isAuthorized =
                filtetPipeline.Select(filterInfo => filterInfo.Instance).Any(filter => filter is IAuthorizationFilter);
            //判断是否匿名方法
            var allowAnonymous = apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();

            if (isAuthorized && !allowAnonymous)
            {
                //operation.parameters.Add(new Parameter
                //{
                //    name = "Authorization",
                //    @in = "header",
                //    description = "访问令牌",
                //    required = true,
                //    type = "string",
                //    @default = "Bearer "
                //});
                operation.parameters.Add(new Parameter
                {
                    name = "Authorization",
                    @in = "header",
                    description = "访问令牌",
                    required = true,
                    type = "string",
                    @default = ""
                });
            }
        }
    }
}