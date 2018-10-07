using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Search
{
    public class BuildSqlReturnModel
    {
        /// <summary>
        /// 产生的SQL
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public List<SugarParameter> ListParameter { get; set; }
    }
}
