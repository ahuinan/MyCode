using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message.Act.Common;
using MyCode.Project.Domain.Message.Act.User;
using MyCode.Project.Domain.Message.Request.User;
using MyCode.Project.Domain.Message.Response.Common;
using MyCode.Project.Domain.Message.Response.Jurisdiction;
using MyCode.Project.Domain.Message.Response.User;
using MyCode.Project.Domain.Model;
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Constant;
using MyCode.Project.Infrastructure.Enumeration;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.Extensions;
using MyCode.Project.Infrastructure.UnityExtensions;
using MyCode.Project.Repositories.Common;

namespace MyCode.Project.Services.Implementation
{
    /// <summary>
    /// 基础资料模块
    /// </summary>
    public class BasicService : IBasicService
    {
        #region 初始化
        private readonly ISysRegionRepository _sysRegionRepository;

        public BasicService(ISysRegionRepository sysRegionRepository)
        {
            _sysRegionRepository = sysRegionRepository;
        }
        #endregion

        #region GetRegionList(根据父级id得到下面的区域数据)
        [CachingCallHandler("00:30:00")]
        public List<KeyValue> GetRegionList(Guid? parentId)
        {
            if (parentId.IsEmpty()) { parentId = Const.DefaultReginId; }

            return _sysRegionRepository.Queryable()
                .Where(p => p.ParentID == parentId && p.Status == (int)Status.Enable)
                .Select(p => new KeyValue() { Text = p.Name, Value = p.RegionID })
                .ToList(); 
        }
        #endregion
    }
}
