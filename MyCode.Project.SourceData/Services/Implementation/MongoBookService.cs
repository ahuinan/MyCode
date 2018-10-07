using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.Infrastructure.Common;
using Wolf.Project.Infrastructure.Exceptions;
using Wolf.Project.SourceData.Domain.Enum;
using Wolf.Project.SourceData.Model;
using Wolf.Project.SourceData.Services.BLL;

namespace Wolf.Project.SourceData.Services.Implementation
{
    public class MongoBookService: IMongoBookService
    {
        private readonly DouBanBLL _douBanBLL;
        private readonly IBookDetailRepository _bookDetailRepository;

        public MongoBookService(DouBanBLL douBanBLL,
            IBookDetailRepository bookDetailRepository)
        {
            _douBanBLL = douBanBLL;
            _bookDetailRepository = bookDetailRepository;
        }

        #region SpliderOneBook(根据isbn获取一本书的基本资料，只处理基本数据，动态数据比如评分由其它调度去执行)
        /// <summary>
        /// 分为：1）基本资料，以豆瓣为主,豆瓣没有，则从国家图书馆找 2)动态资料，分类采用当当网的
        /// </summary>
        /// <param name="isbn"></param>
        public BookDetail SpliderOneBook(string isbn)
        {
            if (string.IsNullOrEmpty(isbn)) { throw new BaseException(); }

            BookDetail bookDetail =  null;

            var listBookFromMongoDB = _bookDetailRepository.SelectList(p => p.isbn10 == isbn || p.isbn13 == isbn);

            //先处理豆瓣的数据
            try
            {
                var doubanBook = listBookFromMongoDB.Find(p => p.source == EnumHelper.GetDescription(DataSource.DouBan));

                if (doubanBook == null)
                {
                    //如果mongodb里面已经有数据了，则不自动抓取
                    bookDetail = _douBanBLL.GetBookDetail(isbn);

                    _bookDetailRepository.Add(bookDetail);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("DouBanError",ex);
                Console.WriteLine(ex);
            }

            //国家图书馆取数据
            if (bookDetail == null)
            {
                return null;
            }

            return bookDetail;
        }
        #endregion


    }
}
