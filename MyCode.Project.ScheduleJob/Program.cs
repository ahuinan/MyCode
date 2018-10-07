using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Wolf.Project.Infrastructure.UnityExtensions;
using Quartz.Unity;

namespace Wolf.Project.ScheduleJob
{
	class Program
	{

		static void Main(string[] args)
		{
			// Container = UnityHelper.BuildUnityContainer();
			

			JobsHelp.start(AppDomain.CurrentDomain.BaseDirectory + "/JobConfig.xml");
	







		
		
		}
	}
}
