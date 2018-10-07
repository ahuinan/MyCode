using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wolf.Project.SourceData.Domain.Model
{
    public class BaseEntity
    {
        public ObjectId id { get; set;}

    }
}
