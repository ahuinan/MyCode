using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Exports;

namespace MyCode.Project.Services.BLL.ReprotExport
{
    public abstract class BaseExport : MarshalByRefObject 
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public object Condition { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        public Guid MerchantId { get; set; }

        /// <summary>
        /// 初始化要导出的属性信息
        /// </summary>
        public abstract List<ExportExcelProperty> GetProperties();

        /// <summary>
        /// Excel保存的名字
        /// </summary>
        protected string ExcelName;

        public BaseExport()
        {
            ExcelName = DateTime.Now.ToString("yyyyMMddHHmmss_fff") + ".xls";
        }

        /// <summary>
        /// 列表数据
        /// </summary>
        public IList List { get; set; }

        #region GetPageSearch(获取分页查询条件)
        /// <summary>
        /// 获取分页查询条件
        /// </summary>
        /// <typeparam name="T">条件类型</typeparam>
        /// <param name="condition">查询条件</param>
        /// <param name="queryCount">查询数量</param>
        /// <returns></returns>
        public PagedSearch<TEntity> GetPageSearch<TEntity>(object condition, int pagesize,int pageIndex = 1) where TEntity:class,new()
        {
            var tCondition = ((JObject)condition).ToObject<TEntity>();
            return new PagedSearch<TEntity>(pagesize, pageIndex, tCondition);
        }
        #endregion

        #region  Execute(执行导出)
        /// <summary>
        /// 执行导出
        /// </summary>
        public virtual void Execute()
        {

            var excelConfig = new ExportExcelConfig();

            excelConfig.Properties.AddRange(GetProperties());

            excelConfig.CreateExcelFile(List, ExcelName);
        }
        #endregion

    }
}
