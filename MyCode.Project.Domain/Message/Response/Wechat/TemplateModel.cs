using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Business.WeiXin
{
    /// <summary>
    /// 模板实体
    /// </summary>
    public class TemplateModel
    {
        /// <summary>
        /// 接受者openid
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 模板跳转链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 模板项数据
        /// </summary>
        public List<TemplateItem> Data { get; set; }

        /// <summary>
        /// 关键字索引
        /// </summary>
        private int _keywordIndex;

        /// <summary>
        /// 初始化一个<see cref="TemplateModel"/>
        /// </summary>
        /// <param name="openid">接受者openid</param>
        /// <param name="templateId">模板ID</param>
        /// <param name="url">模板跳转链</param>
        public TemplateModel(string openid, string templateId, string url="")
        {
            this.Openid = openid;
            this.TemplateId = templateId;
            this.Url = url;
            this.Data=new List<TemplateItem>();
            this._keywordIndex = 0;
        }

        /// <summary>
        /// 添加模板前置语
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public TemplateModel First(string value)
        {
            this.AddData("first", value);
            return this;
        }

        /// <summary>
        /// 添加模板关键字
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public TemplateModel Keyword(string value)
        {
            this._keywordIndex++;
            this.AddData("keyword"+_keywordIndex,value);            
            return this;
        }

        /// <summary>
        /// 添加模板后置语
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public TemplateModel Remark(string value)
        {
            this.AddData("remark", value);
            return this;
        }

        /// <summary>
        /// 添加模板项数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private void AddData(string key, string value)
        {
            this.Data.Add(new TemplateItem(key, value));
        }
    }
}
