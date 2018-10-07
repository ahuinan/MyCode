using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Services;
using MyCode.Project.Domain.Message;
using AutoMapper;
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Domain.Model;
using MyCode.Project.Infrastructure.UnityExtensions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Transactions;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Enumeration;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.OutSideService;
using MyCode.Project.Domain.Config;
using System.Reflection;
using MyCode.Project.Repositories.Common;
using Newtonsoft.Json;
using System.Threading;

namespace MyCode.Project.Services.Implementation
{
    public class WorkProcessService: ServiceBase,IWorkProcessService
    {

        #region 初始化
        private readonly IWorkProcessRepository _workProcessRepository;
        private readonly IUserService _userService;

        private static object locker = new object(); //创建锁

        public WorkProcessService(IWorkProcessRepository workProcessRepository,
            IUserService userService)
		{
			_workProcessRepository = workProcessRepository;
            _userService = userService;
        }
        #endregion

        #region SelectInitWorkProcess（返回前几十条数据）
        public List<WorkProcess> SelectInitWorkProcess(int top) {

            return _workProcessRepository.Queryable()
                .Where(p => p.Status == (int)WorkProcessStatus.Init && p.SystemType == (int)SystemType.Book)
                .Take(top)
                .OrderBy(p => p.UpdateTime).ToList();
        }
		#endregion

        

		#region RestratStopProcess(重新启用所有暂停了的调度，这里不需要事务，因为修改失败也不影响)
		public void RestratStopProcess()
		{
			var list = _workProcessRepository.SelectList(p=>p.Status == (int)WorkProcessStatus.Stop);
			list.ForEach(x => {
				x.Status = (int)WorkProcessStatus.Running;
				x.UpdateTime = DateTime.Now;
				_workProcessRepository.Update(x);

			});
		}
		#endregion

		#region RestartStopProcess（重新启用某个暂停了的调度）
        [TransactionCallHandler]
		public void RestartStopProcess(Guid workprocessId) {
			var workprocess = _workProcessRepository.SelectFirst(p => p.WorkProcessId == workprocessId);

			if (workprocess.Status != (int)WorkProcessStatus.Stop) {throw new BaseException("当前进程状态不是停止");}

			workprocess.Status = (int)WorkProcessStatus.Running;
			workprocess.UpdateTime = DateTime.Now;
			_workProcessRepository.Update(workprocess);

        }
		#endregion

        #region ExecuteSingle(执行单个)
        public void ExecuteSingle(WorkProcess process)
		{

				var type = UnityHelper.GetUnityContainer().Resolve(Type.GetType(process.TypePath));

				MethodInfo method = type.GetType().GetMethod(process.MethodName);

				if (!string.IsNullOrEmpty(process.ParameterInfo))
				{
					method.Invoke(type, new object[] { process.ParameterInfo });
				}
				else
				{
					method.Invoke(type, new object[] { });
				}
            process.Status = (int)WorkProcessStatus.Finished;
            _workProcessRepository.Update(process);

		}
        #endregion

        #region Execute（调度执行）

        public void Execute()
		{
            lock (locker)
            {
                var client = UnityHelper.GetService<MyCodeSqlSugarClient>();

                var list = SelectInitWorkProcess(10);

                //先将这10个任务改成运行中
                if (list != null && list.Count > 0)
                {
                    var updateList = list.Select(p => new WorkProcess { WorkProcessId = p.WorkProcessId, Status = 1 });

                    _workProcessRepository.Update(updateList.ToList(), it => new { it.Status });
                }

                foreach (var process in list)
                {

                    var hashcode = client.GetHashCode();

                    Console.WriteLine("仓储：" + hashcode);
                    Console.WriteLine("0" + client.Ado.Connection.State);

                    client.Ado.BeginTran();

                    #region 执行一个任务
                    try
                    {
                        ExecuteSingle(process);

                        Console.WriteLine("2" + client.Ado.Connection.State);
                    }
                    catch (Exception ex)
                    {
                        while (ex.InnerException != null)
                        {
                            ex = ex.InnerException;
                        }

                        Console.WriteLine(ex);
                        Console.WriteLine(ex.StackTrace);
                        process.Status = (int)WorkProcessStatus.Stop;
                        process.UpdateTime = DateTime.Now;
                        if (ex is BaseException)
                        {
                            process.ExceptionInfo = ex.Message;
                        }
                        else
                        {
                            process.ExceptionInfo = ex.ToString();
                        }
                        _workProcessRepository.Update(process);
                    }
                    #endregion

                    //看下当前Connection状态
                    Console.WriteLine("3" + client.Ado.Connection.State);
                    client.Ado.CommitTran();

                }
            }  
        }
        #endregion

        #region Add(添加调度)

        /// <summary>
        /// 添加调度
        /// </summary>
        /// <typeparam name="T">执行类</typeparam>
        /// <param name="merchantId">商家ID</param>
        /// <param name="methodName">方法名</param>
        /// <param name="remark">备注</param>
        /// <param name="entity">参数信息</param>
        /// <param name="funcType">执行类型</param>
        public void Add<T>(string methodName,  object entity = null, string remark = "", FuncType funcType = FuncType.Function) 
        {
            Add(typeof(T), methodName, remark, entity, funcType);
        }
        #endregion

        #region Add(添加调度)
        /// <summary>
        /// 添加调度
        /// </summary>
        /// <param name="merchantId">商家ID</param>
        /// <param name="type">执行类</param>
        /// <param name="methodName">方法名</param>
        /// <param name="remark">备注</param>
        /// <param name="entity">参数信息</param>
        /// <param name="funcType">执行类型</param>
        public void Add(Type type, string methodName, string remark = "", object entity = null, FuncType funcType = FuncType.Function)
        {
            string typePath = string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
            string paramInfo = entity == null
                ? ""
                : entity is string || entity is Guid || entity is int || entity is long || entity is decimal ||
                  entity is float || entity is double
                    ? entity.ToString()
                    : JsonConvert.SerializeObject(entity);
            Add(typePath, methodName, remark, paramInfo, funcType);
        }
        #endregion

        #region Add(添加调度)
        /// <summary>
        /// 添加调度任务
        /// </summary>
        /// <param name="merchantId">商家ID</param>
        /// <param name="typePath">类型路径，如：Lxm.IServices.IWorkProcessService, Lxm.Services</param>
        /// <param name="methodName">方法名</param>
        /// <param name="remark">备注</param>
        /// <param name="paramInfo">参数信息</param>
        /// <param name="funcType">执行类型</param>
        public void Add(string typePath, string methodName, string remark = "", string paramInfo = "",
            FuncType funcType = FuncType.Function)
        {

            WorkProcess entity = new WorkProcess()
            {
                WorkProcessId = Guid.NewGuid(),
                FuncType = (int)funcType,
                TypePath = typePath,
                MethodName = methodName,
                ParameterInfo = paramInfo,
                Status = (int)WorkProcessStatus.Init,
                Remark = remark,
                SystemType = (int)SystemType.Book,
                UpdateTime = DateTime.Now
            };
           _workProcessRepository.Add(entity);
        }
        #endregion

    }
}
