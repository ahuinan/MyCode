using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCode.Project.Generate.Template
{
    public class TemplateForRepository:BaseTemplate
    {
       

        public TemplateForRepository(string tablename):base(tablename)
        {
            base.SavePath = Path.Combine(FileUtils.GetSolutionPath(), "MyCode.Project.Repositories", $"{tablename}Repository.cs");

            base.TemplateContent = $@"using MyCode.Project.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message;
using MyCode.Project.Domain.Model;
using MyCode.Project.Domain.Repositories;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Search;

namespace MyCode.Project.Repositories
{{
    public class {tablename}Repository: Repository<{tablename}>, I{tablename}Repository
    {{
        public {tablename}Repository(MyCodeSqlSugarClient context) : base(context)
        {{ }}

      


	

	}}
}}";
        }
    }
}
