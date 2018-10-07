using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using MyCode.Project.Domain.Message.Response.Common;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Services;
using MyCode.Project.WebApi.App_Filter;

namespace MyCode.Project.WebApi.Controllers
{
    public class CommonController :BaseAPIController
    {

        private readonly IBasicService _basicService;

        public CommonController(IBasicService basicService)
        {
            _basicService = basicService;
        }

        /// <summary>
        /// 得到区域列表
        /// </summary>
        /// <param name="id">该ID为父级id，如果是要拿中国所有省的数据，可以不传;</param>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValue> GetRegionList(Guid? id)
        {
            return _basicService.GetRegionList(id);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        [IgnoreResultHandle]
        [AllowAnonymous]
        [HttpGet]
        public void ExportExcel()
        {
            var context = CurrentHttpContext;


          
        }

    }
}
