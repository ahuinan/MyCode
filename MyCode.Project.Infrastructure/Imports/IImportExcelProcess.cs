using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 导入Excel数据任务接口
    /// </summary>
    public interface IImportExcelProcess
    {
		
        /// <summary>
        /// 执行导入Excel数据的方法
        /// </summary>
        ImportResult RunImportExcelProcess();

        /// <summary>
        /// 获取导入Excel数据的方法
        /// </summary>
        List<object> GetImportExcelData();
    }
    /// <summary>
    /// 导入数据的处理事件
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="entityList">数据源</param>
    public delegate object ImportEvent<T>(List<T> entityList);

    /// <summary>
    /// 获取导入数据的处理事件
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <param name="entityList">数据源</param>
    /// <returns></returns>
    public delegate List<T> GetDataEvent<T>(List<T> entityList);
}
