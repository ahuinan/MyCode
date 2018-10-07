using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using Microsoft.Practices.Unity.WebApi;
using System.Web.Http;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Microsoft.Practices.Unity.Mvc;
using MyCode.Project.Repositories;
using Microsoft.Practices.Unity.InterceptionExtension;
using MyCode.Project.Infrastructure.UnityExtensions;
using MyCode.Project.Repositories.Common;
using MyCode.Project.Infrastructure.Cache;
using MyCode.Project.Domain.Config;
using MyCode.Project.WebApi.Areas.Admin.Controllers;

namespace MyCode.Project.WebApi
{
	/// <summary>
	/// Specifies the Unity configuration for the main container.
	/// </summary>
	public class UnityConfig
	{
		#region Unity Container
		//private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
		//{
		//	var container = new UnityContainer();
		//	RegisterTypes(container);
		//	return container;
		//});


		public static IUnityContainer Container = GetConfiguredContainer();

		/// <summary>
		/// Gets the configured Unity container.
		/// </summary>
		public static IUnityContainer GetConfiguredContainer()
		{
			return GetContainer();
		}
		#endregion

		/// <summary>Registers the type mappings with the Unity container.</summary>
		/// <param name="container">The unity container to configure.</param>
		/// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
		/// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
		public static IUnityContainer GetContainer()
		{
			var container = UnityHelper.GetUnityContainer();

            //数据库库链接对象为按每一次请求
            container.RegisterType<MyCodeSqlSugarClient>(new PerRequestLifetimeManager());

            //注册缓存对象
            container.RegisterType<IMyCodeCacheService, RedisCache>(new InjectionConstructor(SystemConfig.RedisAddress, SystemConfig.CachePrefix));

            return container;
		}



	}
}