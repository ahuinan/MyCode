using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Model;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Exports;

namespace MyCode.Project.Services.BLL.ReprotExport
{
    public class ExportTestBLL1:BaseExport
    {
        private readonly IJurisdictionService _jurisdictionService;

        public ExportTestBLL1(IJurisdictionService jurisdictionService) :base()
        {
            _jurisdictionService = jurisdictionService;
        }

        #region GetProperties(得到属性)
        public override List<ExportExcelProperty> GetProperties()
        {
            var properties = new List<ExportExcelProperty>();

            properties.Add(new ExportExcelProperty { Caption = "账号", EntityProp = "Login" });

            properties.Add(new ExportExcelProperty { Caption = "姓名",EntityProp = "Name" });

            return properties;
        }
        #endregion

        #region Execute(执行)
        public override void Execute()
        {
            var pageSearch = new PagedSearch();

            int pageIndex = 1;

            while ( 1 == 1)
            {
                var result = _jurisdictionService.GetLoginPageList(pageSearch, base.MerchantId);

                if (result == null || result.DataList == null || result.DataList.Count == 0) { break; }

                base.List = result.DataList;

                base.Execute();

                pageSearch.Page = pageSearch.Page + 1;
            }
        }
        #endregion

    }
}
