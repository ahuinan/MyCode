using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Domain.Model;
using MyCode.Project.Infrastructure.Enumeration;
using MyCode.Project.Domain.Message.Response.Common;

namespace MyCode.Project.Services
{
    public interface IBasicService
    {
        /// <summary>
        /// 根据父级id得到下面的区域数据
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<KeyValue> GetRegionList(Guid? parentId);


    }
}
