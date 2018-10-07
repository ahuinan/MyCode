using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace MyCode.Project.WebApi.SwaggerExtensions
{
    /// <summary>
    /// 认证令牌操作
    /// </summary>
    public class AuthTokenOperation : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            //swaggerDoc.paths.Add("/token", new PathItem()
            //{
            //    post = new Operation
            //    {
            //        tags = new List<string>() { "Auth" },
            //        consumes = new List<string>()
            //        {
            //            "application/x-www-form-urlencoded"
            //        },
            //        parameters = new List<Parameter>()
            //        {
            //            new Parameter()
            //            {
            //                name = "username",
            //                @in = "formData",
            //                required = true,
            //                type = "string",
            //                description = "账号"
            //            },
            //            new Parameter()
            //            {
            //                name = "password",
            //                @in = "formData",
            //                @default = null,
            //                type = "string",
            //                required = false,
            //                description = "密码"
            //            },
            //            new Parameter()
            //            {
            //                name = "grant_type",
            //                @in = "formData",
            //                @default = "password",
            //                required = true,
            //                type = "string",
            //                description = "授权类型,默认password"
            //            },
            //            new Parameter()
            //            {
            //                name="scope",
            //                @in="formData",
            //                required = true,
            //                type = "Array[string]",
            //                description = "登录类型,管理后台:admin,微信:wechat"
            //            }
            //        }
            //    }
            //});
        }
    }
}