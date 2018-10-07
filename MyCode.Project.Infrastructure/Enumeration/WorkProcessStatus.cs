using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Enumeration
{
    public enum WorkProcessStatus
    {
        [Description("初始化")]
        Init = 0,

        [Description("执行中")]
        Running = 1,
	    
        [Description("执行完成")]
        Finished = 9,

        [Description("因错暂停")]
        Stop = 2

    }


}
