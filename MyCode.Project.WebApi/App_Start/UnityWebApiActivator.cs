using System.Web.Http;
using Microsoft.Practices.Unity.WebApi;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Microsoft.Practices.Unity.Mvc;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MyCode.Project.WebApi.App_Start.UnityWebApiActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(MyCode.Project.WebApi.App_Start.UnityWebApiActivator), "Shutdown")]

namespace MyCode.Project.WebApi.App_Start
{
    /// <summary>Provides the bootstrapping for integrating Unity with WebApi when it is hosted in ASP.NET</summary>
    public static class UnityWebApiActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start() 
        {
            // Use UnityHierarchicalDependencyResolver if you want to use a new child container for each IHttpController resolution.
            // var resolver = new UnityHierarchicalDependencyResolver(UnityConfig.GetConfiguredContainer());
        var resolver = new Microsoft.Practices.Unity.WebApi.UnityDependencyResolver(UnityConfig.GetConfiguredContainer());

            GlobalConfiguration.Configuration.DependencyResolver = resolver;

			DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
		}

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}
