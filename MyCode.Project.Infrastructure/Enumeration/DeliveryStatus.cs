using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Project.Infrastructure.Enumeration
{
    public enum DeliveryStatus
    {
        [Description("在途")]
        OnTheWay = 1,

        [Description("派件中")]
        InTheDelivery = 2,

        [Description("已签收")]
        HasSignned = 3,

        [Description("派送失败（拒签等）")]
        SendFailed = 4
    }
}
