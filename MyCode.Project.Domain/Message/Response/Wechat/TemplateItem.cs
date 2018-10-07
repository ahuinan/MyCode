using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Business.WeiXin
{
    /// <summary>
    /// 模板项
    /// </summary>
    public class TemplateItem
    {
        /// <summary>
        /// 模板编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 模板值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 初始化一个<see cref="TemplateItem"/>类型的实例
        /// </summary>
        /// <param name="code">模板编码</param>
        /// <param name="value">模板值</param>
        /// <param name="color">字体颜色，默认#173177</param>
        public TemplateItem(string code, string value, string color = "#173177")
        {
            this.Code = code;
            this.Value = value;
            this.Color = color;
        }
    }
}
