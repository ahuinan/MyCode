using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Extensions;

namespace MyCode.Project.Infrastructure.Exports
{
    /// <summary>
    /// 导出Excel任务
    /// </summary>
    public class ExportExcelProcess:IExportExcelProcess
    {        
        /// <summary>
        /// 导出Excel配置
        /// </summary>
        public ExportExcelConfig Config { get; }

        /// <summary>
        /// 获取导出Excel数据的处理事件
        /// </summary>
        private readonly GetExportDataEvent _exportExcelFunc;

        /// <summary>
        /// 初始化一个<see cref="ExportExcelProcess"/>类型的实例
        /// </summary>
        /// <param name="config"></param>
        /// <param name="func"></param>
        public ExportExcelProcess(ExportExcelConfig config, GetExportDataEvent func)
        {
            this.Config = config;
            this._exportExcelFunc = func;
        }

        /// <summary>
        /// 执行导出Excel数据的方法
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>返回文件路径</returns>
        public string RunExportExcelProcess(string fileName)
        {
            if (_exportExcelFunc != null)
            {
                //导出Excel每次最多的导出数量
                var exportExcelDataCount = ConfigurationManager.AppSettings["ExportExcelDataCount"];
                //默认1000条数据
                int queryCount = 1000;
                if (!string.IsNullOrEmpty(exportExcelDataCount) && Regex.IsMatch(exportExcelDataCount, "^[0-9]+$"))
                {
                    queryCount = int.Parse(exportExcelDataCount);
                }
                //获取导出数据所需的查询条件
                var dataList = this._exportExcelFunc(this.Config.Condition, queryCount);

                var excelFile = this.Config.CreateExcelFile(dataList,fileName);    
                
                return excelFile;
            }
            throw new ApplicationException("没有找到导出Excel数据的方法!");
        }
    }
}
