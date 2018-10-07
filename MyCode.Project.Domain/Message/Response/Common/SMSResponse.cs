using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Domain.Message.Response.Common
{
    public class SMSResponse
    {
        public string code
        {
            get;
            set;
        }

        public string cause
        {
            get;
            set;
        }
    }
}
