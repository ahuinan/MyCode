using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using MyCode.Project.Infrastructure.UnityExtensions;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.OutSideService;
using MyCode.Project.Services;
using System.Threading;

namespace MyCode.Project.ScheduleTask
{
	class Program
	{

		static void Main(string[] args)
		{


            JobsHelp.start(AppDomain.CurrentDomain.BaseDirectory + "/JobConfig.xml");

            Console.ReadKey();

        }
	}
}
