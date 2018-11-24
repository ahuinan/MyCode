using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyCode.Project.Services;
using MyCode.Project.Domain.Message;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Domain.Model;
using MyCode.Project.Infrastructure.Enumeration;
using SqlSugar;
using MyCode.Project.Infrastructure.Search;
using MyCode.Project.Infrastructure.Cache;

namespace MyCode.Project.WebApi.Controllers
{
	/// <summary>
	/// 缓存接口
	/// </summary>
    public class CacheController : BaseAPIController
    {

        private readonly IMyCodeCacheService _cacheService;
      
        public CacheController(IMyCodeCacheService cacheService)
		{
            _cacheService = cacheService;
		}

        /// <summary>
        /// 添加缓存
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public void Add()
        {

            _cacheService.Set("time", DateTime.Now, new TimeSpan(0, 10, 0));
        }

        /// <summary>
        /// 获取缓存Test
        /// </summary>
        [HttpGet]
        public object Get()
        {
            return _cacheService.Get<DateTime?>("time");
        }
	}
}
