using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.Domain.Repositories;
using Wolf.Project.SourceData.Model;
using Wolf.Project.SourceData.Repositories.Common;

namespace Wolf.Project.SourceData.Repositories
{
    public class BookDetailRepository : MongoRepository<BookDetail>,IBookDetailRepository
    {
        public BookDetailRepository(MyMongoClient context) : base(context)
        {

        }
    }
}
