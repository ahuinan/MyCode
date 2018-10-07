using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Common
{
    public class FilesUploadResp
    {
        public Data data
        {
            get;
            set;
        }

        public string message
        {
            get;
            set;
        }

        /// <summary>
        /// 0为正常，其他则为有错误
        /// </summary>
        public int code
        {
            get;
            set;
        }
    }
}
