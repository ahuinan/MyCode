using System;
using System.Web.Http;
using WebActivatorEx;
using MyCode.Project.WebApi;
using Swashbuckle.Application;
using MyCode.Project.WebApi.SwaggerExtensions;
using System.Web.Http.Description;
using MyCode.Project.WebApi.App_Start;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace MyCode.Project.WebApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration 
                .EnableSwagger(c =>
                    {
                        // 配置多API版本
                        c.MultipleApiVersions(ResolveAreasSupportByRouteConstraint, (vc) =>
                        {
                            vc.Version("Admin", "后台API");
                            vc.Version("Wechat", "微信端API");
                            vc.Version("v1", "通用API",true);
                        });

                        // c.SingleApiVersion("v1", "MyCode.Project.WebApi");
                        //c.ApiKey("Authorization").Description("OAuth2 Auth").In("header").Name("Bearer ");
                        c.ApiKey("Authorization").Description("OAuth2 Auth").In("header").Name("");
                        c.IncludeXmlComments(string.Format("{0}/bin/MyCode.Project.WebApi.xml", AppDomain.CurrentDomain.BaseDirectory));
                        c.IncludeXmlComments(string.Format("{0}/bin/MyCode.Project.Domain.xml", AppDomain.CurrentDomain.BaseDirectory));
                        c.IncludeXmlComments(string.Format("{0}/bin/MyCode.Project.Infrastructure.xml", AppDomain.CurrentDomain.BaseDirectory));
                        c.OperationFilter<AddAuthorizationHeaderParameterOperationFilter>();
                        c.OperationFilter<AddUploadOperationFilter>();
                        c.DocumentFilter<AuthTokenOperation>();

                        c.DocumentFilter<SwaggerAreasSupportDocumentFilter>();

                        c.ApiKey("Authorization").Description("OAuth2 Auth").In("header").Name("Authorization");
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.EnableApiKeySupport("Authorization", "header");
                    });
        }

        /// <summary>
        /// 解析区域路由
        /// </summary>
        /// <param name="apiDescription"></param>
        /// <param name="targetApiVersion"></param>
        /// <returns></returns>
        private static bool ResolveAreasSupportByRouteConstraint(ApiDescription apiDescription, string targetApiVersion)
        {
            if (targetApiVersion == "v1")
            {
                return apiDescription.Route.RouteTemplate.StartsWith("api/{controller}");
            }
            var routeTemplateStart = "api/" + targetApiVersion;
            return apiDescription.Route.RouteTemplate.StartsWith(routeTemplateStart, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
