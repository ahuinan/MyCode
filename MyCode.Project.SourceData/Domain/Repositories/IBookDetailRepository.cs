using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Project.Domain.Repositories;
using Wolf.Project.SourceData.Model;

namespace Wolf.Project.SourceData
{
    public interface IBookDetailRepository : IMongoDBRepository<BookDetail>
    {
    }
}
