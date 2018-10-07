using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.Infrastructure.Exceptions;
using Wolf.Project.Infrastructure.Extensions;
using Wolf.Project.Infrastructure.WebPost;
using Wolf.Project.SourceData.Model;

namespace Wolf.Project.SourceData.Services.BLL
{
    /// <summary>
    /// 国家图书馆相关
    /// </summary>
    public class LibraryBLL
    {
        /// <summary>
        /// 不需要授权公开api可以使用http，参数里面如果不包含apikey的话，限制单ip每小时150次
        /// </summary>
        private static string _apiUrl = "https://api.douban.com";

        private readonly IBookDetailRepository _bookDetailRepository;
        private readonly IMongoWorkProcessService _mongoWorkProcessService;

        public LibraryBLL(IBookDetailRepository bookDetailRepository,
            IMongoWorkProcessService mongoWorkProcessService) {

            _bookDetailRepository = bookDetailRepository;
            _mongoWorkProcessService = mongoWorkProcessService;
        }

        #region GetBookDetail(根据ID得到书的明细)
        public BookDetail GetBookDetail(int id)
        {
            var methodUrl = $"{_apiUrl}/v2/book/{id}";

            var webUtils = new WebUtils();

            string proxyIp = "";

            var response = webUtils.DoGet(methodUrl, null, out proxyIp, true);

            var result = JObject.Parse(response);

            var code = result.GetSingle<string>("code");

            if (!string.IsNullOrEmpty(code)) { throw new BaseException($"接口查询出错:{methodUrl},{result.GetSingle<string>("msg")}"); }

            return null;
            //return ToBookDetail(result);
        }
        #endregion

        #region GetBookDetail(根据isbn码得到图书资料) 
        public BookDetail GetBookDetail(string isbn)
        {
            return null;
        }
        #endregion

    }
}
