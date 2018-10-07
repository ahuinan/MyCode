using Quartz;
using System;
using System.Data;
using System.Data.Common;
using Wolf.Project.Services;
using Wolf.Project.Infrastructure;
using Wolf.Project.Infrastructure.UnityExtensions;
using Microsoft.Practices.Unity;
using System.Reflection;
using Wolf.Project.Domain.Model;
using Wolf.Project.Domain.ViewModel;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Wolf.Project.ScheduleTask.Jobs
{
	public class CalculateBeiBeiRechargeJob : IJob
	{

		private readonly IRechargeCommissionService _rechargeCommissionService;


		public CalculateBeiBeiRechargeJob(IRechargeCommissionService rechargeCommissionService)
		{
            _rechargeCommissionService = rechargeCommissionService;
		}



		public void Execute(IJobExecutionContext context)
		{
            _rechargeCommissionService.AddCalculateBeiBeiRechargeCommissionProcess();
		}
	}
}
