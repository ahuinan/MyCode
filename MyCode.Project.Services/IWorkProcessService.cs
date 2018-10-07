using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Domain.Model;
using MyCode.Project.Infrastructure.Enumeration;

namespace MyCode.Project.Services
{
    public interface IWorkProcessService
    {
		/// <summary>
		/// 执行单个的调度
		/// </summary>
		/// <param name="process"></param>
		//void ExecuteSingle(Guid processId);

		List<WorkProcess> SelectInitWorkProcess(int top);

        /// <summary>
        /// 执行，每次执行10条数据
        /// </summary>
		void Execute();

        /// <summary>
        /// 重启失败调度
        /// </summary>
		void RestratStopProcess();

		/// <summary>
		/// 重新启用某个暂停了的调度
		/// </summary>
		/// <param name="workprocessId"></param>
		void RestartStopProcess(Guid workprocessId);

        /// <summary>
        /// 添加调度
        /// </summary>
        /// <typeparam name="T">命名空间类</typeparam>
        /// <param name="methodName">执行方法</param>
        /// <param name="remark">备注</param>
        /// <param name="entity">传参</param>
        /// <param name="funcType">默认为函数</param>
        void Add<T>(string methodName, object entity = null, string remark = "", FuncType funcType = FuncType.Function);


    }
}
