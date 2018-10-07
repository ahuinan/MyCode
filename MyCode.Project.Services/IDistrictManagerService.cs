using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.Domain.ViewModel;
using Wolf.Project.Domain.Message;
using Wolf.Project.Infrastructure.Common;
using Wolf.Project.Domain.Message.Request;
using Wolf.Project.Domain.Message.Act;
using Wolf.Project.Domain.Message.Response;

namespace Wolf.Project.Services
{
    public interface IDistrictManagerService
    {

        /// <summary>
        /// 添加区域管理人
        /// </summary>
        /// <param name="act">请求参数</param>
        /// <param name="merchantId">商家ID</param>
        /// <param name="username">用户名</param>
        void AddDistrictManager(DistrictManagerAct act, Guid merchantId, string username);

        /// <summary>
        /// 区域管理人修改
        /// </summary>
        /// <param name="act">请求参数</param>
        /// <param name="merchantId">商家ID</param>
        /// <param name="username">用户名</param>
        void UpdateDistrictManager(DistrictManagerAct act, Guid merchantId, string username);

        /// <summary>
        /// 启用/禁用区域管理人
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="username">用户名</param>
        void ChangeDistrictManagerStatus(ChangeStatusRequest request, string username);

        /// <summary>
        /// 根据DistrictManagerId得到的区域管理
        /// </summary>
        /// <param name="request">依赖于DistrictManagerId</param>
        /// <returns></returns>
        PageResult<DistrictManagerInResp> GetDistrictManagerInPageList(PagedSearch<DistrictManagerInDependOnIdSearchRequest> request);

        /// <summary>
        /// 区域管理人列表
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="merchantId">商家ID</param>
        /// <returns></returns>
        PageResult<DistrictManagerListResp> GetDistrictManagerPageList(PagedSearch<DistrictManagerSearchRequest> request, Guid merchantId);

        /// <summary>
        /// 根据customerid和groupId得到区域内管理信息
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="customerId">分销商ID</param>
        /// <returns></returns>
        PageResult<DistrictManagerInResp> GetDistrictManagerInPageList(PagedSearch<Guid> request, Guid customerId);
    }
}
