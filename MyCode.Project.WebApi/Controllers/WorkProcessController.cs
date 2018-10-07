using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyCode.Project.Services;
using MyCode.Project.Domain.Message;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Common;

namespace MyCode.Project.WebApi.Controllers
{
	/// <summary>
	/// 调度接口
	/// </summary>
    public class WorkProcessController : BaseAPIController
    {
		private readonly IWorkProcessService _workProcessService;


		public WorkProcessController(IWorkProcessService workProcessService)
		{
			_workProcessService = workProcessService;
		}


		/// <summary>
		/// 修复问题后重新开启停止了的某个调度
		/// </summary>
		[AllowAnonymous]
		[HttpGet]

		public void RestratStopProcess(Guid id)
		{
            _workProcessService.RestartStopProcess(id);
            _workProcessService.RestartStopProcess(id);
		}


	}
}
