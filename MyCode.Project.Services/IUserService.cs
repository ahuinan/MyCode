using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Domain.Model;
using MyCode.Project.Domain.Message.Response.User;

namespace MyCode.Project.Services
{
    public interface IUserService
    {
		/// <summary>
		/// 执行单个的调度
		/// </summary>
		/// <param name="process"></param>
		void AddUser();

        /// <summary>
        /// 管理员根据账号和密码登陆
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        LoginInfo AdminLogin(string account, string password);

        /// <summary>
        /// 根据授权Key来登陆，用于微信授权等场景
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        LoginInfo MemberLogin(string key);

    }
}
