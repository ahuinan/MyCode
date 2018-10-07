using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure
{

    public class Result
    {
        public Object Data { get; set; }
        public ResultCode ResultCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    
}