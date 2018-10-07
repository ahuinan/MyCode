using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wolf.Project.Domain.Message;
using Wolf.Project.Domain.Model;
using Wolf.Project.Domain.Repositories;
using Wolf.Project.Domain.ViewModel;
using Wolf.Project.Infrastructure.Constant;
using Wolf.Project.Infrastructure.Exceptions;
using Wolf.Project.Infrastructure.UnityExtensions;
using Wolf.Project.Services.BLL;
using AutoMapper;
using JCE.Utils.AutoMapper;
using Wolf.Project.Domain.Config;
using Newtonsoft.Json;
using Wolf.Project.Infrastructure.Enumeration;
using Wolf.Project.Infrastructure.Extensions;
using System.Web.Script.Serialization;
using Wolf.Project.Domain.Message.Act;
using Wolf.Project.Infrastructure.Common;
using Wolf.Project.Domain.Message.Request;
using Wolf.Project.Domain.Message.Response;

namespace Wolf.Project.Services.Implementation
{
    public class DistrictManagerService : ServiceBase, IDistrictManagerService, IDisposable
    {
        private readonly IBasDistrictManagerRepository _basDistrictManagerRepository;

        public DistrictManagerService(IBasDistrictManagerRepository basDistrictManagerRepository) {

            _basDistrictManagerRepository = basDistrictManagerRepository;
        }

        #region ValidateDistrictManger(区域管理人保存验证)
        private void ValidateDistrictManger(DistrictManagerAct act)
        {
            ValidBusiness(act.GroupId == Guid.Empty, "团队没有选择");
            ValidBusiness(act.CustomerId == Guid.Empty, "区域管理人没有选择");
            ValidBusiness(act.DistrictId == Guid.Empty, "区域没有选择");
        }
        #endregion

        #region AddDistrictManager(添加区域管理人)
        [TransactionCallHandler]
        public void AddDistrictManager(DistrictManagerAct act,Guid merchantId,string username)
        {

            ValidateDistrictManger(act);

            var managerCount = _basDistrictManagerRepository.Count(p => p.DistributorGroupID == act.GroupId && p.CustomerID == act.CustomerId);

            ValidBusiness(managerCount > 0,"该分销商存在管理的区域");

            var districtManager = new BasDistrictManager()
            {
                ID = Guid.NewGuid(),
                MerchantID = merchantId,
                DistributorGroupID = act.GroupId,
                CustomerID = act.CustomerId,
                State = act.StateId,
                City = act.CityId,
                District = act.DistrictId,
                Status = 0,
                Creater = username,
                CreateTime = DateTime.Now,
                Editor = username,
                EditTime = DateTime.Now
            };

            _basDistrictManagerRepository.Add(districtManager);
        }
        #endregion

        #region UpdateDistrictManager(区域管理人修改)
        [TransactionCallHandler]
        public void UpdateDistrictManager(DistrictManagerAct act, Guid merchantId, string username)
        {
            ValidateDistrictManger(act);

            var managerCount = _basDistrictManagerRepository.Count(p => p.DistributorGroupID == act.GroupId && p.CustomerID == act.CustomerId && p.ID != act.Id);

            ValidBusiness(managerCount > 0, "该区域已经存在管理人");

            var districtManager = _basDistrictManagerRepository.SelectFirst(p => p.ID == act.Id);
            districtManager.Status = 0;
            districtManager.DistributorGroupID = act.GroupId;
            districtManager.CustomerID = act.CustomerId;
            districtManager.State = act.StateId;
            districtManager.City = act.CityId;
            districtManager.District = act.DistrictId;
            districtManager.Editor = username;
            districtManager.EditTime = DateTime.Now;

            _basDistrictManagerRepository.Update(districtManager);
        }
        #endregion

        #region ChangeDistrictManagerStatus(启用/禁用区域管理人)
        [TransactionCallHandler]
        public void ChangeDistrictManagerStatus(ChangeStatusRequest request,string username)
        {
            ValidBusiness(request.ListID == null || request.ListID.Count == 0, "没有选中id");
            ValidBusiness(request.Status != 1 && request.Status != 0, "状态传值只能为1或0");

            var districtManagers = request.ListID.Select(line => new BasDistrictManager { ID = line,Status = request.Status,Editor = username,EditTime = DateTime.Now}).ToList();

            _basDistrictManagerRepository.UpdateList(districtManagers,x => x.ColumnsToUpdate( c => c.Status,x1 => x1.EditTime,x2 => x2.Editor));
        }
        #endregion

        #region GetDistrictManagerPageList(区域管理人列表)
        public PageResult<DistrictManagerListResp> GetDistrictManagerPageList(PagedSearch<DistrictManagerSearchRequest> request, Guid merchantId)
        {
            return _basDistrictManagerRepository.GetDistrictManagerPageList(request, merchantId);
        }
        #endregion

        #region GetDistrictManagerInPageList(根据DistrictManagerId得到的区域管理)
        public PageResult<DistrictManagerInResp> GetDistrictManagerInPageList(PagedSearch<DistrictManagerInDependOnIdSearchRequest> request)
        {
            var condition = request.Condition;

            ValidBusiness(condition.DistricutManagerId == null, "DistricutManagerId参数必传");

            var districtManager = _basDistrictManagerRepository.SelectQueryableData(p => p.ID == condition.DistricutManagerId)
                .Select(p => new { p.DistributorGroupID,p.CustomerID,p.District})
                .FirstOrDefault();

            ValidBusiness(districtManager == null,"不存在该DistrictManager的数据");

            var districtManagerAreaInSearchRequest = new DistrictManagerAreaInSearchRequest()
            {
                GroupId = districtManager.DistributorGroupID,
                ManagerCustomerId = districtManager.CustomerID,
                IntroducerOrWechat = condition.IntroducerOrWechat,
                NameOrWechat = condition.NameOrWechat,
                DistrictId = districtManager.District,
                GradeId = condition.GradeId
            };

            var newRequest = new PagedSearch<DistrictManagerAreaInSearchRequest>()
            {
                Condition = districtManagerAreaInSearchRequest,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return _basDistrictManagerRepository.GetDistrictManagerInPageList(newRequest);
        }
        #endregion

        #region GetDistrictManagerInPageList(根据customerid和groupId得到区域内管理信息)
        public PageResult<DistrictManagerInResp> GetDistrictManagerInPageList(PagedSearch<Guid> request,Guid customerId)
        {
            var groupId = request.Condition;

            var districtManager = _basDistrictManagerRepository.SelectQueryableData(p => p.CustomerID == customerId && p.DistributorGroupID == groupId && p.Status == 1)
                                                                .Select(p => new { p.CustomerID,p.District})
                                                                .FirstOrDefault();

            if (districtManager == null) { return null; }

            var districtManagerAreaInSearchRequest = new DistrictManagerAreaInSearchRequest()
            {
                GroupId = groupId,
                ManagerCustomerId = customerId,
                DistrictId = districtManager.District
            };

            var newRequest = new PagedSearch<DistrictManagerAreaInSearchRequest>()
            {
                Condition = districtManagerAreaInSearchRequest,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return _basDistrictManagerRepository.GetDistrictManagerInPageList(newRequest);
        }
        #endregion
    }
}


