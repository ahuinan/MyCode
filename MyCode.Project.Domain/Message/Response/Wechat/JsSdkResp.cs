using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Wechat
{
    public class JsSdkResp
    {
        public string AppId
        {
            get;
            set;
        }

        public string Timestamp
        {
            get;
            set;
        }

        public string NonceStr
        {
            get;
            set;
        }

        public string Signature
        {
            get;
            set;
        }
    }
}
