using Quartz;
using System;
using System.Data;
using System.Data.Common;
using Wolf.Project.Services;
using Wolf.Project.Infrastructure;
using Wolf.Project.Infrastructure.UnityExtensions;
using Microsoft.Practices.Unity;

namespace Wolf.Project.ScheduleJob
{
	public class TestJob : IJob
    {

		private readonly IOrderService _orderService;

		public TestJob(IOrderService orderService) {
			_orderService = orderService;
		}
		public void Execute(IJobExecutionContext context)
        {
            try
            {
				//IUnityContainer container = UnityHelper.GetUnityContainer();
				//IOrderService OrderService = container.Resolve<IOrderService>();

				Console.WriteLine(_orderService.GetCurrentTime());
				//var test = container.Resolve<CachingCallHandler>(new ParameterOverride("expirationTime", this.ExpirationTime));
				//IOrderService OrderService = 

				//throw new Exception("ERROR");
				// Console.WriteLine("TEST1执行时间：" + _orderService.GetCurrentTime());

			
			}
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        
        }
    }
}

