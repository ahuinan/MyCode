using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Quartz;

namespace MyCode.Project.ScheduleTask.UnityQuartz
{
	public class QuartzUnityExtension : UnityContainerExtension
	{
		protected override void Initialize()
		{
			this.Container.RegisterType<ISchedulerFactory, UnitySchedulerFactory>(new ContainerControlledLifetimeManager());
			this.Container.RegisterType<IScheduler>(new InjectionFactory(c => c.Resolve<ISchedulerFactory>().GetScheduler()));
		}
	}
}
