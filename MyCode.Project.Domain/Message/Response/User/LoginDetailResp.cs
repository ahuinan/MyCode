using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.User
{
    public class LoginDetailResp
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
        /// 角色ID
        /// </summary>
        public Guid? RoleId { get; set; }



    }
}
