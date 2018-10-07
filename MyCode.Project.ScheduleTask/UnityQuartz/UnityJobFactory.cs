using System;
using System.Globalization;
using Common.Logging;
using Microsoft.Practices.Unity;
using Quartz.Spi;
using Quartz;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Repositories.Common;
using MyCode.Project.Infrastructure.UnityExtensions;

namespace MyCode.Project.ScheduleTask.UnityQuartz
{
	public class UnityJobFactory : IJobFactory
	{

		private readonly IUnityContainer container;

		static UnityJobFactory()
		{
		}

		public UnityJobFactory(IUnityContainer container)
		{
			this.container = container;
		}

		public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
		{
			var jobDetail = bundle.JobDetail;
			var jobType = jobDetail.JobType;

			try
			{
				//if (Log.IsDebugEnabled)
				//{
				//	Log.Debug(string.Format(
				//		CultureInfo.InvariantCulture,
				//		"Producing instance of Job '{0}', class={1}", new object[] { jobDetail.Key, jobType.FullName }));
				//}

				return typeof(IInterruptableJob).IsAssignableFrom(jobType)
					? new InterruptableJobWrapper(bundle, container)
					: new JobWrapper(bundle, container);
			}
			catch (Exception ex)
			{
				throw new SchedulerException(string.Format(
					CultureInfo.InvariantCulture,
					"Problem instantiating class '{0}'", new object[] { jobDetail.JobType.FullName }), ex);
			}
		}

		public void ReturnJob(IJob job)
		{
			// Nothing here. Unity does not maintain a handle to container created instances.
		}


		#region Job Wrappers


		internal class JobWrapper : IJob
		{
			private readonly TriggerFiredBundle bundle;
			private readonly IUnityContainer unityContainer;

			/// <summary>
			///     Initializes a new instance of the <see cref="T:System.Object" /> class.
			/// </summary>
			public JobWrapper(TriggerFiredBundle bundle, IUnityContainer unityContainer)
			{
				if (bundle == null)
				{
					throw new ArgumentNullException("bundle");
				}

				if (unityContainer == null)
				{
					throw new ArgumentNullException("unityContainer");
				}

				this.bundle = bundle;
				this.unityContainer = unityContainer;
			}

			protected IJob RunningJob { get; private set; }


			public void Execute(IJobExecutionContext context)
			{
                var childContainer = unityContainer.CreateChildContainer();
				try
				{
					Console.WriteLine(DateTime.Now + " : Start to run the job - " + bundle.JobDetail.Key.Name);
					RunningJob = (IJob)childContainer.Resolve(bundle.JobDetail.JobType);
					RunningJob.Execute(context);
				}
				//catch (JobExecutionException x)
				//{
				//	throw;
				//}
				catch (Exception ex)
				{
					Console.WriteLine(string.Format(CultureInfo.InvariantCulture,
						"Failed to execute Job '{0}' of type '{1}'",
						bundle.JobDetail.Key, bundle.JobDetail.JobType));
					Console.WriteLine(ex);
                    LogHelper.Error("RUNNINNGTASK",  ex);

				}
				finally
				{

                    var sqlSugarClient = UnityHelper.GetService<MyCodeSqlSugarClient>();

                    if (sqlSugarClient != null) { sqlSugarClient.Context.Ado.Dispose(); }

                    Console.WriteLine("1:" + sqlSugarClient.Ado.Connection.State + ",GetHashCode:" + sqlSugarClient.GetHashCode());
                    RunningJob = null;
					childContainer.Dispose();
				}
			}
		}

		internal sealed class InterruptableJobWrapper : JobWrapper, IInterruptableJob
		{
			public InterruptableJobWrapper(TriggerFiredBundle bundle, IUnityContainer unityContainer)
				: base(bundle, unityContainer)
			{
			}

			public void Interrupt()
			{
				var interruptableJob = RunningJob as IInterruptableJob;

				if (interruptableJob != null)
				{
					interruptableJob.Interrupt();
				}
			}
		}

		#endregion
	}
}
