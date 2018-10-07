using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Common
{
    public class KeyValue
    {
        /// <summary>
        /// 显示的文字
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public Guid Value { get; set; }
    }
}
