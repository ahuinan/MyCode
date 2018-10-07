using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.SourceData.Model;

namespace Wolf.Project.SourceData.Services
{
    public interface IMongoBookService
    {
        /// <summary>
        /// 根据Isbn号，可能为10或者13位获取书籍信息并写入MongoDB;只获取基本资料
        /// </summary>
        /// <param name="isbn"></param>
        BookDetail SpliderOneBook(string isbn);


    }
}
