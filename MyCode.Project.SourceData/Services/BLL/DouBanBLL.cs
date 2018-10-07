using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.Infrastructure.Common;
using Wolf.Project.Infrastructure.WebPost;
using Wolf.Project.SourceData.Domain.Enum;
using Wolf.Project.SourceData.Model;
using NSoup;
using NSoup.Nodes;
using Wolf.Project.Infrastructure.Exceptions;
using Wolf.Project.Infrastructure.Extensions;
using MongoDB.Bson;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using Wolf.Project.Domain.Model;
using Wolf.Project.Infrastructure.Enumeration;
using Wolf.Project.Domain.Repositories;
using Wolf.Project.Services;
using Wolf.Project.OutSideService;

namespace Wolf.Project.SourceData.Services.BLL
{
    /// <summary>
    /// 豆瓣相关
    /// </summary>
    public class DouBanBLL
    {
        /// <summary>
        /// 不需要授权公开api可以使用http，参数里面如果不包含apikey的话，限制单ip每小时150次
        /// </summary>
        private static string _apiUrl = "https://api.douban.com";

        private readonly IBookDetailRepository _bookDetailRepository;
        private readonly IMongoWorkProcessService _mongoWorkProcessService;
        private readonly IMongoWorkProcessRepository _mongoWorkProcessRepository;

        public DouBanBLL(IBookDetailRepository bookDetailRepository,
            IMongoWorkProcessService mongoWorkProcessService,
            IMongoWorkProcessRepository mongoWorkProcessRepository)
        {
            _bookDetailRepository = bookDetailRepository;
            _mongoWorkProcessService = mongoWorkProcessService;
            _mongoWorkProcessRepository = mongoWorkProcessRepository;
        }

        #region ToBookDetail(将字符串处理成BookDetail)
        private BookDetail ToBookDetail(JObject result)
        {
            var detail = new BookDetail();
            detail.isbn10 = result.GetSingle<string>("isbn10");
            detail.isbn13 = result.GetSingle<string>("isbn13");
            detail.source = EnumHelper.GetDescription(DataSource.DouBan);
            detail.sourceurl = result.GetSingle<string>("alt");
            detail.binding = result.GetSingle<string>("binding");
            detail.authors = result.GetList<string>("author");
            detail.category0 = "";
            detail.category1 = "";
            detail.coverimg = result.GetSingle<string>("images", "small");
            detail.levelnum = result.GetSingle<string>("rating", "average");
            detail.origintitle = result.GetSingle<string>("origin_title");
            detail.pages = result.GetSingle<string>("pages");
            detail.price = result.GetSingle<string>("price");
            detail.pubdate = result.GetSingle<string>("pubdate");
            detail.publisher = result.GetSingle<string>("publisher");
            detail.subtitle = result.GetSingle<string>("subtitle");
            detail.summary = result.GetSingle<string>("summary");
            detail.catalog = result.GetSingle<string>("catalog");
            detail.title = result.GetSingle<string>("title");
            detail.translators = result.GetList<string>("translator");
            detail.tags = result.GetList<string>("tags", "name");
            detail.createtime = DateTime.Now;
            detail.updatetime = DateTime.Now;

            //记录到mongoDB
            Console.WriteLine($"处理文章:{detail.title}");
            Console.WriteLine(JsonConvert.SerializeObject(detail));

            return detail;
        }
        #endregion

        #region GetBookDetail(根据ID得到书的明细)
        public BookDetail GetBookDetail(int id)
        {
            var methodUrl = $"{_apiUrl}/v2/book/{id}";

            var webUtils = new WebUtils();

            string proxyIp = "";

            var response = webUtils.DoGet(methodUrl, null, out proxyIp, true);

            var result = JObject.Parse(response);

            var code = result.GetSingle<string>("code");

            if (!string.IsNullOrEmpty(code)) { throw new BaseException($"豆瓣接口查询出错:{methodUrl},{result.GetSingle<string>("msg")}"); }

            return ToBookDetail(result);
        }
        #endregion

        #region GetBookDetail(根据isbn得到豆瓣的数信息)
        public BookDetail GetBookDetail(string isbn)
        {
            var methodUrl = $"{_apiUrl}/v2/book/isbn/:{isbn}";

            var webUtils = new WebUtils();

            string proxyIp = "";

            var response = webUtils.DoGet(methodUrl, null,out proxyIp,false);

            var result = JObject.Parse(response);

            var code = result.GetSingle<string>("code");

            if (!string.IsNullOrEmpty(code)) { throw new BaseException($"豆瓣接口查询出错:{methodUrl},{result.GetSingle<string>("msg")}"); }

            return ToBookDetail(result);
        }
        #endregion

        #region HandleOneBookById(根据豆瓣一本书的ID采集进MongoDB)
        public void HandleOneBookById(object objId)
        {
            int id = int.Parse(objId.ToString());

            var bookDetail = GetBookDetail(id);

            var bookDetailFromDB = _bookDetailRepository.SelectFirst(p => (p.isbn13 == bookDetail.isbn13 || 
            p.isbn10 == bookDetail.isbn10) 
            && p.source == EnumHelper.GetDescription(DataSource.DouBan));

            if (bookDetailFromDB != null) { return; }

            _bookDetailRepository.Add(bookDetail);
        }
        #endregion

        #region SpiderAllBook(按关键字抓取)
        public void SpiderAllBook(string tag)
        {

            #region 暂时注释抓取排名前250条数据
            ////for (int i = 0; i < 10; i++)
            ////{
            ////    var url = $"https://book.douban.com/top250?start={i*25}";


            ////    var web = new HtmlWeb();

            ////    var doc = web.Load(url);

            ////    var nodes = doc.DocumentNode.SelectNodes(@"//a[@class='nbg']");

            ////    var workProcessList = new List<WorkProcess>();

            ////    foreach (HtmlNode node in nodes)
            ////    {
            ////        string href = node.GetAttributeValue("href", "");

            ////        //提取数字部分
            ////        var id = Regex.Replace(href, @"[^0-9]+", "");

            ////        Console.WriteLine($"抓到豆瓣数据，书id:{id}");

            ////        _mongoWorkProcessService.Add<DouBanBLL>("HandleOneBookById", id, "豆瓣TOP250");

            ////    }

            #endregion 

            int i = 0;

            while (1 == 1)
            {
                var url = $"https://book.douban.com/tag/{tag}?start={i * 20}&type=T";

                var web = new HtmlWeb();

                var doc = web.Load(url);

                var nodes = doc.DocumentNode.SelectNodes(@"//a[@class='nbg']");

                var workProcessList = new List<WorkProcess>();

                if (nodes == null) { break; }

                foreach (HtmlNode node in nodes)
                {
                    string href = node.GetAttributeValue("href", "");

                    //提取数字部分
                    var id = Regex.Replace(href, @"[^0-9]+", "");

                    Console.WriteLine($"抓到豆瓣数据，书id:{id}");

                    //如果之前调度已经有了该条数据，则不需要再加进去
                    var count = _mongoWorkProcessRepository.Count(p => p.methodname == "HandleOneBookById" && p.parameterinfo == id);

                    if (count > 0) { continue; }

                    _mongoWorkProcessService.Add<DouBanBLL>("HandleOneBookById", id, "按Tag标签抓取");
                }

                i++;
            }
        }
        #endregion

        #region GetAllTags(得到豆瓣所有标签)
        public List<string> GetAllTags()
        {
            string url = "https://book.douban.com/tag/?view=cloud";

            var tags = new List<string>();

            var web = new HtmlWeb();

            var doc = web.Load(url);

            var nodes = doc.DocumentNode.SelectNodes(@"//table//tbody//tr//td//a[contains(@href,'tag')]");

            foreach (var node in nodes)
            {
                Console.WriteLine($"开始抓取分类{node.InnerText}的分类书籍数据");

                SpiderAllBook(node.InnerText);

                System.Threading.Thread.Sleep(60000);

            }
            return tags;
        }
        #endregion

    }


}
