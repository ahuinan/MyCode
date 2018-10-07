using Microsoft.Practices.Unity;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wolf.Project.Infrastructure.Common;
using Wolf.Project.Infrastructure.Enumeration;
using Wolf.Project.Infrastructure.Exceptions;
using Wolf.Project.Infrastructure.UnityExtensions;
using Wolf.Project.SourceData.Domain.Enum;
using Wolf.Project.SourceData.Domain.Model;
using Wolf.Project.SourceData.Model;
using Wolf.Project.SourceData.Services.BLL;

namespace Wolf.Project.SourceData.Services.Implementation
{
    public class MongoWorkProcessService: IMongoWorkProcessService
    {

        private readonly IMongoWorkProcessRepository _mongoWorkProcessRepository;

        public MongoWorkProcessService(IMongoWorkProcessRepository mongoWorkProcessRepository)
        {
            _mongoWorkProcessRepository = mongoWorkProcessRepository;
        }

        #region RestratStopProcess(重新启用所有暂停了的调度，这里不需要事务，因为修改失败也不影响)
        public void RestratStopProcess()
        {
            var list = _mongoWorkProcessRepository.SelectList(p => p.status == (int)WorkProcessStatus.Stop);
            list.ForEach(x => {
                x.status = (int)WorkProcessStatus.Running;
                x.updatetime = DateTime.Now;
                _mongoWorkProcessRepository.Update(x);

            });
        }
        #endregion

        #region RestartStopProcess（重新启用某个暂停了的调度）
        public void RestartStopProcess(ObjectId workprocessId)
        {
            var workprocess = _mongoWorkProcessRepository.SelectFirst(p => p.id == workprocessId);

            if (workprocess.status != (int)WorkProcessStatus.Stop) { throw new BaseException("当前进程状态不是停止"); }

            workprocess.status = (int)WorkProcessStatus.Running;
            workprocess.updatetime = DateTime.Now;
            _mongoWorkProcessRepository.Update(workprocess);

        }
        #endregion

        #region ExecuteSingle(执行单个)
        public void ExecuteSingle(MongoWorkProcess process)
        {
            var taskdesc = $"{DateTime.Now}:执行任务{process.typepath}{process.methodname}{process.parameterinfo}";

            Console.WriteLine($"开始:{taskdesc}");

            process.updatetime = DateTime.Now;

            //多线程执行
            try
            {
                var type = UnityHelper.GetUnityContainer().Resolve(Type.GetType(process.typepath));

                MethodInfo method = type.GetType().GetMethod(process.methodname);

                if (!string.IsNullOrEmpty(process.parameterinfo))
                {
                    method.Invoke(type, new object[] { process.parameterinfo });
                }
                else
                {
                    method.Invoke(type, new object[] { });
                }

                process.status = (int)WorkProcessStatus.Finished;
                _mongoWorkProcessRepository.Update(process);
                Console.WriteLine($"执行成功:{taskdesc}");
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                //Console.WriteLine(ex);
                //Console.WriteLine(ex.StackTrace);
                process.status = (int)WorkProcessStatus.Stop;

                if (ex is BaseException)
                {
                    process.exceptionInfo = ex.Message;
                }
                else
                {
                    process.exceptionInfo = ex.ToString();
                }
                _mongoWorkProcessRepository.Update(process);
                Console.WriteLine($"出错:{taskdesc}{ex}");

            }
           

        }
        #endregion

        #region Execute（调度执行）

        public void Execute()
        {

                //每次只处理20条数据
                var list = _mongoWorkProcessRepository.Queryable(p => p.status == (int)WorkProcessStatus.Init).SortBy(p => p.updatetime).Limit(1).ToList();

                if (list.Count == 0)
                {
                    list = _mongoWorkProcessRepository.Queryable(p => p.status == (int)WorkProcessStatus.Running).SortBy(p => p.updatetime).Limit(1).ToList();

                    //移除那些不到5分钟的数据
                    list.RemoveAll(p => p.updatetime > DateTime.Now.AddMinutes(-5));
                }

                if (list.Count == 0)
                {
                    list = _mongoWorkProcessRepository.Queryable(p => p.status == (int)WorkProcessStatus.Stop).SortBy(p => p.updatetime).Limit(1).ToList();
                }
            
                ////if (list.Count == 0) {

                ////    Console.WriteLine($"{DateTime.Now}:没有需要处理的数据，暂停1分钟");

                ////    System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));

                ////    continue;
                ////}

                foreach (var process in list)
                {
                    process.status = (int)WorkProcessStatus.Running;
                    process.updatetime = DateTime.Now;
                    _mongoWorkProcessRepository.Update(process);
                }

                var cancellationTokenSource = new CancellationTokenSource();

                var tasks = new List<Task>();

                foreach (var process in list)
                {
                    tasks.Add(
                        Task.Factory.StartNew(() =>
                        {
                            ExecuteSingle(process);
                        }, cancellationTokenSource.Token)
                    );
                }

                Task.WaitAll(tasks.ToArray(), cancellationTokenSource.Token);

            
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
        public void Add<T>(string methodName, object entity = null, string remark = "")
        {
            Add(typeof(T), methodName, remark, entity);
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
        public void Add(Type type, string methodName, string remark = "", object entity = null)
        {
            string typePath = string.Format("{0}, {1}", type.FullName, type.Assembly.GetName().Name);
            string paramInfo = entity == null
                ? ""
                : entity is string || entity is Guid || entity is int || entity is long || entity is decimal ||
                  entity is float || entity is double
                    ? entity.ToString()
                    : JsonConvert.SerializeObject(entity);
            Add(typePath, methodName, remark, paramInfo);
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
        public void Add(string typePath, string methodName, string remark = "", string paramInfo = "")
        {

            var entity = new MongoWorkProcess()
            {
                typepath = typePath,
                methodname = methodName,
                parameterinfo = paramInfo,
                status = (int)WorkProcessStatus.Init,
                remark = remark,
                updatetime = DateTime.Now
            };

            _mongoWorkProcessRepository.Add(entity);
        }
        #endregion


    }
}
