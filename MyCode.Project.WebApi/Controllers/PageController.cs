using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wolf.Project.Domain.Message.Response.Common;
using Wolf.Project.Services;

namespace Wolf.Project.WebApi.Controllers
{
    public class PageController : BaseAPIController
    {

        /// <summary>
        /// 输出Excel
        /// </summary>
        [HttpGet]
        public void OutPutExcel()
        {
            var str = "aa";
        }
    }
}