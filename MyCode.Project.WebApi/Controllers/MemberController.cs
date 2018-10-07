using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wolf.Project.Infrastructure.Exceptions;
using Wolf.Project.Domain.ViewModel;
using Wolf.Project.Services;
using Wolf.Project.Domain.Message;
using Wolf.Project.Infrastructure.Common;
using Wolf.Project.Domain.Model;
using Wolf.Project.Infrastructure.Extensions;
using Wolf.Project.Domain.Message.Response;

namespace Wolf.Project.WebApi.Controllers
{
    /// <summary>
    /// 会员相关信息
    /// </summary>
    public class MemberController : BaseAPIController
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        /// <summary>
        /// 得到会员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MemberInfoResp GetMemberInfo()
        {
            return _memberService.GetMemberInfo(CloudShopInfo.MemberId);
        }


    }
}