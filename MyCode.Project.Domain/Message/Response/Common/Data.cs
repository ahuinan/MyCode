using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Common
{
    public class Data
    {
        /// <summary>
		/// 返回上传的路径
		/// </summary>
		public string key
        {
            get;
            set;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long fsize
        {
            get;
            set;
        }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string link
        {
            get;
            set;
        }
    }
}
