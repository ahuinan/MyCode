using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.SourceData.Model;

namespace Wolf.Project.SourceData.Services
{
    public interface IMongoWorkProcessService
    {
        /// <summary>
        /// 调度执行
        /// </summary>
        void Execute();

        /// <summary>
        /// 添加调度执行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methodName"></param>
        /// <param name="entity"></param>
        /// <param name="remark"></param>
        void Add<T>(string methodName, object entity = null, string remark = "");
    }
}
