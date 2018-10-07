
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using Microsoft.Practices.Unity;
using MyCode.Project.Infrastructure.UnityExtensions;
using Quartz.Impl.Triggers;
using MyCode.Project.ScheduleTask.UnityQuartz;
using MyCode.Project.Repositories;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Repositories.Common;
using MyCode.Project.Infrastructure.Cache;
using MyCode.Project.Domain.Config;

namespace MyCode.Project.ScheduleTask
{

	public static class JobsHelp
	{
		private static string ConfigFile = "";
		private static IScheduler sched = null;

		public static void start(string _ConfigFile)
		{
            IUnityContainer container = UnityHelper.GetUnityContainer();

            container.RegisterType<MyCodeSqlSugarClient>(new PerThreadLifetimeManager());

            //注册缓存对象
            container.RegisterType<IMyCodeCacheService, RedisCache>(new InjectionConstructor(SystemConfig.RedisAddress, SystemConfig.CachePrefix));

            container.AddNewExtension<QuartzUnityExtension>();

            ConfigFile = _ConfigFile;
			List<jobinfo> list = new List<jobinfo>();
			try
			{
				if (sched != null)
				{
					stop();
					sched = null;
				}
				//sched = new StdSchedulerFactory().GetScheduler();
				sched = container.Resolve<IScheduler>();
				XmlDocument document = new XmlDocument();
				document.Load(ConfigFile);

				XmlNode node = document.SelectSingleNode("Jobs");
				if (node.ChildNodes.Count > 0)
				{
					foreach (XmlNode node2 in node.ChildNodes)
					{
						jobinfo item = new jobinfo
						{
							name = node2.Attributes["name"].Value,
							type = node2.Attributes["type"].Value,
							CronExpression = node2.Attributes["CronExpression"].Value,
							enabled = bool.Parse(node2.Attributes["enabled"].Value)
						};
						if (item.enabled)
						{
							list.Add(item);
							IJobDetail jobDetail = JobBuilder.Create(Type.GetType(item.type)).WithIdentity(item.name, item.name + "Group").Build();
							ITrigger trigger = TriggerBuilder.Create().WithIdentity(item.name, item.name + "Group").WithCronSchedule(item.CronExpression).Build();

							sched.ScheduleJob(jobDetail, trigger);
						}
					}
					if (list.Count > 0)
					{
                        sched.Start();
					}
					else
					{
						Console.WriteLine("暂未有计划任务开启1");
					}
				}
				else
				{
					Console.WriteLine("暂未有计划任务开启");
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(JsonConvert.SerializeObject(list));
				Console.WriteLine(exception);
                LogHelper.Error("出错", exception);

			}

			Console.ReadKey();
		}

		public static void stop()
		{
			try
			{
				if (sched != null)
				{
					sched.Shutdown(false);
					sched.Clear();
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine("关闭计划任务失败：" + exception.Message);
			}
		}

		private class jobinfo
		{
			public string CronExpression { get; set; }

			public bool enabled { get; set; }

			public string name { get; set; }

			public string type { get; set; }
		}
	}
}

