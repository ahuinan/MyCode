using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.Domain.Repositories;
using Wolf.Project.SourceData.Domain.Model;
using Wolf.Project.SourceData.Model;
using Wolf.Project.SourceData.Repositories.Common;

namespace Wolf.Project.SourceData.Repositories
{
    public class MongoWorkProcessRepository : MongoRepository<MongoWorkProcess>,IMongoWorkProcessRepository
    {
        public MongoWorkProcessRepository(MyMongoClient context) : base(context)
        {

        }
    }
}
