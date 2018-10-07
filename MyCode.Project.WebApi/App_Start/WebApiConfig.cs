using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using MyCode.Project.WebApi.App_Filter;
using MyCode.Project.WebApi.Providers;
using System.Web.Http.Dispatcher;
using Newtonsoft.Json;

namespace MyCode.Project.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			config.Filters.Add(new ExceptionHandleAttribute());
			config.Filters.Add(new ResultHandleAttribute());
            

            // Web API 配置和服务
            // 将 Web API 配置为仅使用不记名令牌身份验证。
           // config.SuppressDefaultHostAuthentication();

            config.Filters.Add(new AuthorizeFilter());
            //config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
    

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));

            config.Routes.AddAreaRoute("Admin", "MyCode.Project.WebApi.Areas.Admin.Controllers");
            config.Routes.AddAreaRoute("Wechat", "MyCode.Project.WebApi.Areas.Wechat.Controllers");

            config.Routes.MapHttpRoute(
               name: "CommonApi",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new
               {
                   action = RouteParameter.Optional,
                   id = RouteParameter.Optional,
                   namespaces = new string[] { "MyCode.Project.WebApi.Controllers" }
               }
           );

            Formater(config);


        }

        #region AddAreaRoute(添加路由)
        /// <summary>
        /// 添加路由
        /// </summary>
        /// <param name="routes">路由集合</param>
        /// <param name="name">路由名</param>
        /// <param name="namespaces">命名空间</param>
        private static void AddAreaRoute(this HttpRouteCollection routes, string name, string namespaces)
        {
            routes.MapHttpRoute(
                name: name + "Api",
                routeTemplate: "api/" + name + "/{controller}/{action}/{id}",
                defaults: new
                {
                    action = RouteParameter.Optional,
                    id = RouteParameter.Optional,
                    namespaces = new string[] { namespaces }
                });
        }
        #endregion

        #region Formater(响应参数格式化)
        /// <summary>
        /// 响应参数格式化
        /// </summary>
        /// <param name="config">Http配置</param>
        private static void Formater(HttpConfiguration config)
        {
            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//全局处理 返回时间格式
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;//全局处理 接收时间并做本地化处理
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();//首字母小写驼峰式命名
        }
        #endregion
    }
}
