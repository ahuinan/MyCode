using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Exports
{
    /// <summary>
    /// 导出Excel任务接口
    /// </summary>
    public interface IExportExcelProcess
    {
        /// <summary>
        /// 执行导出Excel数据的方法
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>返回文件路径</returns>
        string RunExportExcelProcess(string fileName);
    }

    /// <summary>
    /// 获取导出Excel数据的处理事件
    /// </summary>
    /// <param name="condition">查询条件</param>
    /// <param name="queryCount">查询数量</param>
    /// <returns></returns>
    public delegate IList GetExportDataEvent(object condition,int queryCount);
}
