using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.User
{
    public class LoginListResp
    {
        /// <summary>
        /// 账号ID
        /// </summary>
        public Guid LoginId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tele { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

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
