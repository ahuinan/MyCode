using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Exceptions;

namespace MyCode.Project.Infrastructure.Imports
{
    /// <summary>
    /// 导入Excel数据任务
    /// </summary>
    public class ImportExcelProcess<T>:IImportExcelProcess where T:class ,new()
    {
        /// <summary>
        /// 导入数据对应的配置实体
        /// </summary>
        public ImportConfig<T> Config { get; protected set; }

        /// <summary>
        /// 导入数据方法
        /// </summary>
        private ImportEvent<T> ImportFunc;

        /// <summary>
        /// 获取数据方法
        /// </summary>
        private GetDataEvent<T> GetDataFunc;

        /// <summary>
        /// 初始化一个<see cref="ImportExcelProcess{T}"/>类型的实例
        /// </summary>
        /// <param name="config">导入配置</param>
        /// <param name="func">导入方法</param>
        public ImportExcelProcess(ImportConfig<T> config, ImportEvent<T> func)
        {
            this.Config = config;
            this.ImportFunc = func;
        }

        /// <summary>
        /// 初始化一个<see cref="ImportExcelProcess{T}"/>类型的实例
        /// </summary>
        /// <param name="config">导入配置</param>
        /// <param name="func">获取数据方法</param>
        public ImportExcelProcess(ImportConfig<T> config, GetDataEvent<T> func)
        {
            this.Config = config;
            this.GetDataFunc = func;
        }

        /// <summary>
        /// 执行导入Excel数据的方法
        /// </summary>
        public ImportResult RunImportExcelProcess()
        {
            if (ImportFunc != null)
            {
                ImportResult result=new ImportResult();
                var msg = "";
                if (!this.Config.ValidFormat(ref msg))
                {
                    throw new BaseException(msg);
                }
                //获取Excel数据
                var sourceData = this.Config.ExcelUtil.ExcelToList();

                var count = sourceData.Count/400;
                if (sourceData.Count < 400)
                {
                    count = 1;
                }
                else if (sourceData.Count%400 > 0)
                {
                    count++;
                }
                //如果数据量太多会出现导入失败的问题，暂时先用分批导入解决问题
                for (int i = 0; i < count; i++)
                {
                    var buffer = sourceData.Skip(i*400).Take(400).ToList();
                    var resultData = this.Config.GetListData(buffer);
                    if (resultData != null)
                    {
                        var tmpResult = this.ImportFunc(resultData);
                        var importResult = tmpResult as ImportResult;
                        if (importResult != null)
                        {
                            result.InsertCount += importResult.InsertCount;
                            result.UpdateCount += (tmpResult as ImportResult).UpdateCount;
                        }
                    }
                    else
                    {
                        return result;
                    }                    
                }
				return result;
				
            }
            throw new BaseException("没有找到导入数据的方法!");
        }

        /// <summary>
        /// 获取导入Excel数据的方法
        /// </summary>
        public List<object> GetImportExcelData()
        {
            if (GetDataFunc != null)
            {
                var msg = "";
                if (!this.Config.ValidFormat(ref msg))
                {
                    throw new BaseException(msg);
                }
                //获取Excel数据
                var sourceData = this.Config.ExcelUtil.ExcelToList();

                List<object> result=new List<object>();

                var count = sourceData.Count / 400;
                if (sourceData.Count < 400)
                {
                    count = 1;
                }
                else if (sourceData.Count % 400 > 0)
                {
                    count++;
                }
                //如果数据量太多会出现导入失败的问题，暂时先用分批导入解决问题
                for (int i = 0; i < count; i++)
                {
                    var buffer = sourceData.Skip(i * 400).Take(400).ToList();
                    var resultData = this.Config.GetListData(buffer);
                    if (resultData != null)
                    {
                        var tmpResult = this.GetDataFunc(resultData);
                        if (tmpResult != null)
                        {
                            result.AddRange(tmpResult);
                        }
                        else
                        {
                            return result;
                        }                        
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            throw new BaseException("没有找到导入数据的方法!");
        }        
    }
}
