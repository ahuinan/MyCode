using Quartz;
using System;
using System.Data;
using System.Data.Common;

namespace Wolf.Project.ScheduleJob
{
	public class Test2Job : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            try
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("TEST2执行时间：" + DateTime.Now);
                Console.WriteLine("-------------------------------");
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
    }
}

