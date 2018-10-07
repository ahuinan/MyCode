using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MyCode.Project.Infrastructure.UnityExtensions;
using System.IO;
using MyCode.Project.Infrastructure.Enumeration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace MyCode.Project.Infrastructure.UnityExtensions
{
	public class UnityHelper
	{

        private static IUnityContainer _unityContainer = GetUnityContainer();


        public static IUnityContainer GetUnityContainer()
		{
			if (_unityContainer == null)
			{
				_unityContainer = BuildUnityContainer();
			}
			return _unityContainer;
		}

		public static T GetService<T>()
		{
			return _unityContainer.Resolve<T>();
		}

		private static bool _isWeb = false;

		public static Func<Type, LifetimeManager> GetLifetimeManager()
		{
			if (_isWeb)
			{
				return t => new PerRequestLifetimeManager();
			}
			return t => new TransientLifetimeManager();
		}

		public static IUnityContainer BuildUnityContainer()
		{
			var container = new UnityContainer();

			var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = AppDomain.CurrentDomain.BaseDirectory + "bin\\Unity.xml" };
			Configuration configuration =
				ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");

			container.LoadConfiguration(unitySection);
			//命令行控制台的获取方式
			var dllAll = AllClasses.FromAssembliesInBasePath(false, false, true);

			if (dllAll.Count() == 0)
			{
				//web的方式
				_isWeb = true;
				
				dllAll = AllClasses.FromLoadedAssemblies(false, false, false, true);
			}
			var dllResp = dllAll.Where(t => (t.Namespace == "MyCode.Project.Repositories" || t.Namespace == "MyCode.Project.SourceData.Repositories") && t.Name != "Repository`1" );

			var dllService = dllAll.Where(t => t.Namespace == "MyCode.Project.Services.Implementation" || t.Namespace == "MyCode.Project.SourceData.Services.Implementation");

			var dllOutSide = dllAll.ToList().FindAll(p => p.Namespace != null && p.Namespace.Contains("MyCode.Project.OutSideService"));

			var dllBll = dllAll.Where(t => t.Namespace == "MyCode.Project.Services.BLL" || t.Namespace == "MyCode.Project.SourceData.Services.BLL" || t.Namespace == "MyCode.Project.SourceData.Services.BLL.ReprotExport");


			//注入仓储
			container.RegisterTypes(
				dllResp,
				WithMappings.FromMatchingInterface,
				WithName.Default,
				getLifetimeManager: GetLifetimeManager()
			);

			//注入BLL
			container.RegisterTypes(
				dllBll,
				WithMappings.FromMatchingInterface,
				WithName.Default,
				getLifetimeManager: GetLifetimeManager()
			);

			//注入service
			container.RegisterTypes(
				dllService,
				WithMappings.FromMatchingInterface,
				WithName.Default,
				getInjectionMembers: t => new InjectionMember[]
				{
					new Interceptor<InterfaceInterceptor>(),
					new InterceptionBehavior<ExceptionLogBehavior>(),
					new InterceptionBehavior<PolicyInjectionBehavior>()
				},
				getLifetimeManager: GetLifetimeManager()
			);
			//注入OutSide
			container.RegisterTypes(
				dllOutSide,
				WithMappings.FromMatchingInterface,
				WithName.Default,
				getInjectionMembers: t => new InjectionMember[]
				{
					new Interceptor<InterfaceInterceptor>(),
					new InterceptionBehavior<ExceptionLogBehavior>(),
					new InterceptionBehavior<PolicyInjectionBehavior>()
				},
				getLifetimeManager: GetLifetimeManager()
			);

			return container;

		}
	}
}
