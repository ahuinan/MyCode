using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.Domain.Config;

namespace Wolf.Project.SourceData.Repositories.Common
{
    public class MyMongoClient : MongoClient,IDisposable
    {
        private static MongoUrl config = new MongoUrl(SystemConfig.MongoDBConnectionString) ;

        public MyMongoClient():base(config)
        {
          
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
