using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Project.SourceData.Domain.Enum
{
    public enum DataSource
    {
        /// <summary>
        /// 豆瓣
        /// </summary>
        [Description("douban")]
        DouBan = 1,

        /// <summary>
        /// 国家图书馆
        /// </summary>
        [Description("library")]
        Library = 2
    }
}
