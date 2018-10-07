using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Jurisdiction
{
    /// <summary>
    /// 角色列表
    /// </summary>
    public class RoleListResp
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set;}

        /// <summary>
        /// 修改人
        /// </summary>
        public string Editor { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime EditTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

    }
}
