/************************************************************************************
 * Copyright (c) 2017 All Rights Reserved. 
 * CLR版本：4.0.30319.42000
 * 机器名称：JIAN
 * 命名空间：Wolf.Project.WebApi.Controllers
 * 文件名：CommonController
 * 版本号：v1.0.0.0
 * 唯一标识：323d1bf7-e520-4040-8fa3-ca029aa3521d
 * 当前的用户域：JIAN
 * 创建人：简玄冰
 * 电子邮箱：jianxuanhuo1@126.com
 * 创建时间：2017/6/20 10:06:56
 * 描述：
 *
 * =====================================================================
 * 修改标记：
 * 修改时间：2017/6/20 10:06:56
 * 修改人：简玄冰
 * 版本号：v1.0.0.0
 * 描述：
 *
/************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Wolf.Project.Domain.Message;
using Wolf.Project.Domain.Message.Act;
using Wolf.Project.Domain.Message.Common;
using Wolf.Project.Domain.Message.Request;
using Wolf.Project.Domain.Message.Response;
using Wolf.Project.Domain.ViewModel;
using Wolf.Project.Domain.ViewModel.Common;
using Wolf.Project.Infrastructure;
using Wolf.Project.Infrastructure.Common;
using Wolf.Project.Infrastructure.Enumeration;
using Wolf.Project.Infrastructure.Exceptions;
using Wolf.Project.Services;
using Wolf.Project.WebApi.App_Filter;
using Wolf.Project.WebApi.SwaggerExtensions;
using ImportResult = Wolf.Project.Infrastructure.Imports.ImportResult;

namespace Wolf.Project.WebApi.Controllers
{
    /// <summary>
    /// 区域管理 相关API
    /// </summary>
    public class DistrictManagerController : BaseAPIController
    {
        private readonly IDistrictManagerService _districtManagerService;
      

        public DistrictManagerController(IDistrictManagerService districtManagerService)
        {
            _districtManagerService = districtManagerService;
        }

        /// <summary>
        /// 保存区域管理人
        /// </summary>
        /// <param name="act"></param>
        [HttpPost]
        public void SaveDistrictManager(DistrictManagerAct act)
        {
            if (act.Id == null)
            {
                _districtManagerService.AddDistrictManager(act, CurrentUser.MerchantID, UserName);
            }
            else {
                _districtManagerService.UpdateDistrictManager(act, CurrentUser.MerchantID, UserName);
            }
        }

        /// <summary>
        /// 启用、禁用管理人
        /// </summary>
        /// <param name="request"></param>
        /// <param name="username"></param>
        [HttpPost]
        public void ChangeDistrictManagerStatus(ChangeStatusRequest request)
        {
            _districtManagerService.ChangeDistrictManagerStatus(request, UserName);
        }

        /// <summary>
        /// 得到区域管理人列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns></returns>
        [HttpPost]
        public PageResult<DistrictManagerListResp> GetDistrictManagerPageList(PagedSearch<DistrictManagerSearchRequest> request)
        {
            return _districtManagerService.GetDistrictManagerPageList(request, CurrentUser.MerchantID);
        }

        /// <summary>
        /// 得到区域内代理，适用于后台，因可以得到DistrictManagerId
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <returns></returns>
       [HttpPost]
        public PageResult<DistrictManagerInResp> GetDistrictManagerInPageList(PagedSearch<DistrictManagerInDependOnIdSearchRequest> request)
        {
            return _districtManagerService.GetDistrictManagerInPageList(request);
        }

        /// <summary>
        /// 得到区域内代理，适用于H5微信端
        /// </summary>
        /// <param name="request">团队ID</param>
        [HttpPost]
        public PageResult<DistrictManagerInResp> GetCustomersInDistrictManagerPageList(PagedSearch<Guid> request)
        {
            return _districtManagerService.GetDistrictManagerInPageList(request,CurrentUser.CustomerID.Value);
        }
    }
}