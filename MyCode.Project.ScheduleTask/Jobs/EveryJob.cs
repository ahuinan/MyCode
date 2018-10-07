using Quartz;
using System;
using System.Data;
using System.Data.Common;
using MyCode.Project.Services;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.UnityExtensions;
using Microsoft.Practices.Unity;
using System.Reflection;
using MyCode.Project.Domain.Model;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Threading;

namespace MyCode.Project.ScheduleTask.Jobs
{
	public class EveryJob : IJob
    {

		private readonly IWorkProcessService _workProcessService;

		public EveryJob(IWorkProcessService workProcessService)
		{
			_workProcessService = workProcessService;
		}


		public void Execute(IJobExecutionContext context)
        {
            //Thread thread1 = new Thread(new ThreadStart(_workProcessService.Execute));
            //           //调用Start方法执行线程
            //  thread1.Start();

            Console.WriteLine("worprocessService:" + _workProcessService.GetHashCode());

            _workProcessService.Execute();
		}
    }
}

