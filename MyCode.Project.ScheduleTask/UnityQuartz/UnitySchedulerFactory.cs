using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz.Core;
using Quartz.Impl;
using Quartz;

namespace MyCode.Project.ScheduleTask.UnityQuartz
{
	public class UnitySchedulerFactory : StdSchedulerFactory
	{
		private readonly UnityJobFactory unityJobFactory;

		public UnitySchedulerFactory(UnityJobFactory unityJobFactory)
		{
			this.unityJobFactory = unityJobFactory;
		}

		protected override IScheduler Instantiate(QuartzSchedulerResources rsrcs, QuartzScheduler qs)
		{
			qs.JobFactory = this.unityJobFactory;
			
			return base.Instantiate(rsrcs, qs);
		}
	}
}
