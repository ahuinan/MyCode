using Microsoft.Practices.Unity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using MyCode.Project.Domain.Config;

namespace MyCode.Project.Repositories.Common
{
   
    public class MyCodeSqlSugarClient : SqlSugar.SqlSugarClient
    {

        public MyCodeSqlSugarClient() : base(config)
        {

        }

        private static ConnectionConfig config = new ConnectionConfig()
        {
            ConnectionString = SystemConfig.ConnectionMasterStr,
            DbType = DbType.SqlServer,
            InitKeyType = InitKeyType.Attribute,
            IsAutoCloseConnection = true
           
        };


      

       

        
    }
}
