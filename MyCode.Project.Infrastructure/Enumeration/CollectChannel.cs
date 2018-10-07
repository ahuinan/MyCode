using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Enumeration
{
    /// <summary>
    /// 数据采集来源
    /// </summary>
    public enum CollectChannel
    {
        [Description("豆瓣")]
        DouBan = 1,

        [Description("当当")]
        DangDang = 2,

        [Description("国家图书馆")]
        CountryLibrary = 3
          
    }


}
