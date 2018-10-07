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
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Domain.Model;
using MyCode.Project.Infrastructure.Enumeration;
using SqlSugar;
using MyCode.Project.Infrastructure.Search;
using MyCode.Project.Infrastructure.Format;
using MyCode.Project.Domain.Message.Response.User;
using MyCode.Project.Services.BLL.ReprotExport;

namespace MyCode.Project.WebApi.Controllers
{
	/// <summary>
	/// 测试接口
	/// </summary>
    public class TestController : BaseAPIController
    {

        private readonly IWorkProcessRepository _workProcessRepository;
        private readonly ISysLoginRepository _sysLoginRepository;
        private readonly IJurisdictionService _jurisdictionService;
        private readonly ExportTestBLL1 _exportTestBLL1;

        public TestController(IWorkProcessRepository workProcessRepository,
            ISysLoginRepository sysLoginRepository,
            IJurisdictionService jurisdictionService,
            ExportTestBLL1 exportTestBLL1)
		{
            _workProcessRepository = workProcessRepository;
            _sysLoginRepository = sysLoginRepository;
            _jurisdictionService = jurisdictionService;
            _exportTestBLL1 = exportTestBLL1;

        }

        /// <summary>
        /// 
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public void TestLog()
        {
            LogHelper.Error("错误测试str");
            LogHelper.Error("错误测试Exception",new Exception("ERROR") {});
        }

        [HttpGet]
        public LoginInfo UserCurrent()
        {
            return CurrentLogin;
        }

        [HttpGet]
        [AllowAnonymous]
        public void ExportTest()
        {
            _exportTestBLL1.Execute();
        }

        [AllowAnonymous]
        [HttpGet]
        public string TestDecimal()
        {
            decimal m = 1.7665222m;

            return m.ToTwoPoint();
        }

        [AllowAnonymous]
        [HttpGet]
        public void SelectRunningWorkProcess(int top)
        {
            var list = _sysLoginRepository.SelectList<SysLogin>("select top 10 * from SysLogin where login like '%' + @key + '%'", new { key = "k" });

            // return _workProcessRepository.SelectList<WorkProcess>("Select top 10 * from workprocess where systemtype=200");

            // var workProcessList = _workProcessRepository.Queryable().In<WorkProcess>(p => p.WorkProcessId, "6B2E752C-CBD3-4C56-80C0-0000339F982A", "E1CD8853-993C-4EFE-8863-0000FDF68054");

            // var workProcessList = _workProcessRepository.Queryable().In<WorkProcess>("workprocessid", "6B2E752C-CBD3-4C56-80C0-0000339F982A", "E1CD8853-993C-4EFE-8863-0000FDF68054")

            // var list = _workProcessRepository.SelectList(ids);

            //var comments = lines.Select(line => new Comment { Id = int.Parse(line[0]), Reads = int.Parse(line[1]) });
            //lines.Select(line => new BasQRCode { QRCodeID = line, Note = "测试" }).ToList()

            //var ids = new List<Guid>();
            //ids.Add(Guid.Parse("6B2E752C-CBD3-4C56-80C0-0000339F982A"));
            //ids.Add(Guid.Parse("E1CD8853-993C-4EFE-8863-0000FDF68054"));

            //var work = _workProcessRepository.Queryable().In(ids).Select(p => new { p.WorkProcessId, p.TypePath }).ToList();

            //var workprocesses = lines.Select(line => new WorkProcess { WorkProcessId = line.WorkProcessId, TypePath = "MyCode" });

            //_workProcessRepository.Add(workprocesses.ToList(),it => new { it.TypePath});

            //var list = new List<dynamic>();
            //list.Add(new { WorkProcessId = Guid.Parse("E1CD8853-993C-4EFE-8863-0000FDF68054"), TypePath = "test" });
            //list.Add(new { WorkProcessId = Guid.Parse("6B2E752C-CBD3-4C56-80C0-0000339F982A"), TypePath = "test" });

            //_workProcessRepository.Update(list);

            // var model = _workProcessRepository.Count(p => p.SystemType == 200);

            // List<IConditionalModel> conModels = new List<IConditionalModel>();
            // conModels.Add(new ConditionalModel() { FieldName = "id", ConditionalType = ConditionalType.Equal, FieldValue = "1" });
            // conModels.Add(new ConditionalModel() { FieldName = "id", ConditionalType = ConditionalType.Like, FieldValue = "1" });

            // //conModels.Add(new ConditionalModel() { d})

            // //var searchObj = new SearchCondition();
            // //searchObj.AddCondition("methodname", "cal'", SqlOperator.Like, true);

            // //var search = searchObj.BuildConditionSql();

            // var strSql = "Select * from workprocess" ;

            //var pageData =  _workProcessRepository.SelectListPage<WorkProcess>(strSql, 1, 10, "workprocessid", true);

          

        }
        /// <summary>
        /// 修复问题后重新开启停止了的某个调度
        /// </summary>
        [AllowAnonymous]
		[HttpGet]

		public void Add()
		{
            _workProcessRepository.BeginTran();
            try
            {
                var workProcessList = new List<WorkProcess>();

                for (int i = 0; i < 1000; i++)
                {
                    workProcessList.Add(new WorkProcess()
                    {
                        FuncType = 1,
                        MethodName = "",
                        ParameterInfo = "",
                        WorkProcessId = Guid.NewGuid(),
                        UpdateTime = DateTime.Now
                    });
                }
                _workProcessRepository.Add(workProcessList);
                _workProcessRepository.CommitTran();
            }
            catch (Exception ex)
            {
                _workProcessRepository.CommitTran();
            }

        }
	}
}
