using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyCode.Project.Infrastructure.UnityExtensions;
using System.Web.Http.Dispatcher;
using MyCode.Project.WebApi.Providers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Utility;
using MyCode.Project.WebApi.OAuth;
using System.Web.SessionState;
using MyCode.Project.WebApi.App_Filter;

namespace MyCode.Project.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);


			log4net.Config.XmlConfigurator.Configure();
    


		}

	}
}
