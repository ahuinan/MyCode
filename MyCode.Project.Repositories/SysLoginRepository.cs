using MyCode.Project.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message;
using MyCode.Project.Domain.Model;
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Search;
using MyCode.Project.Domain.Message.Response.User;
using SqlSugar;

namespace MyCode.Project.Repositories
{
    public class SysLoginRepository: Repository<SysLogin>, ISysLoginRepository
    {
        public SysLoginRepository(MyCodeSqlSugarClient context) : base(context)
        { }

        #region GetLoginPageList(得到账户分页列表)
        public PageResult<LoginListResp> GetLoginPageList(PagedSearch request, Guid merchantId)
        {
            var strSql = $@"SELECT	
	L.LoginId,
	Login,
	L.Name,
	Tele,
	L.Editor,
	L.EditTime,
	L.Status,
	SR.Name AS RoleName
FROM 
	SysLogin L WITH (nolock)
LEFT JOIN
	dbo.SysLoginRole LR WITH (NOLOCK) ON L.LoginID = LR.LoginID
LEFT JOIN
	dbo.SysRole SR WITH (nolock) ON LR.RoleID = SR.RoleID";


            var where = new SearchCondition();
            
            where.AddCondition("L.MerchantID",merchantId,SqlOperator.Equal, true);
            //where.AddSqlCondition("L.Login like '%' + @key + '%'",
            //    true,
            //    new SugarParameter("key", "a")); 

            //where.AddCondition("L.status",1,SqlOperator.Equal,true);

            return this.SelectListPage<LoginListResp>(strSql, where, request.Page, request.PageSize, "EditTime desc");
        }
        #endregion




    }
}