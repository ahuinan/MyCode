using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.SourceData.Domain.Model;

namespace Wolf.Project.SourceData.Model
{
    public class BookDetail:BaseEntity
    {
        /// <summary>
        /// 13位的ISBN码
        /// </summary>
        public string isbn13 { get; set; }

        /// <summary>
        /// 10位的ISBN码
        /// </summary>
        public string isbn10 { get; set; }

        /// <summary>
        /// 来源 douban library
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// 来源地址
        /// </summary>
        public string sourceurl { get; set; }

        /// <summary>
        /// 翻译
        /// </summary>
        public List<string> translators { get; set; }

        /// <summary>
        /// 书籍名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string subtitle { get; set; }

        /// <summary>
        /// 源标题（国外源标题）
        /// </summary>
        public string origintitle { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public List<string> authors { get; set; }

        /// <summary>
        /// 出版社名称
        /// </summary>
        public string publisher { get; set; }

        /// <summary>
        /// 发版日期
        /// </summary>
        public string pubdate { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public string pages { get; set; }

        /// <summary>
        /// 装订方式
        /// </summary>
        public string binding { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string coverimg { get; set; }

        /// <summary>
        /// 内容简介
        /// </summary>
        public string summary { get; set; }

        /// <summary>
        /// 目录
        /// </summary>
        public string catalog { get; set; }

        /// <summary>
        /// 销售价格
        /// </summary>
        public string price { get; set; }

        /// <summary>
        /// 推荐指数
        /// </summary>
        public string levelnum { get; set; }

        /// <summary>
        /// 1级分类
        /// </summary>
        public string category0 { get; set; }

        /// <summary>
        /// 2级分类
        /// </summary>
        public string category1 { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public List<string> tags { get; set; }

        /// <summary>
        /// 该数据创建时间
        /// </summary>
        public DateTime createtime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime updatetime { get; set; }
    }
}
