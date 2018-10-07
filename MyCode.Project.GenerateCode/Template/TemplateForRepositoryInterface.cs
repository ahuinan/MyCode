using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCode.Project.Generate.Template
{
    public class TemplateForRepositoryInterface:BaseTemplate
    {
       

        public TemplateForRepositoryInterface(string tablename):base(tablename)
        {

            base.SavePath = Path.Combine(FileUtils.GetSolutionPath(), "MyCode.Project.Domain", "Repositories", $"I{tablename}Repository.cs");

            base.TemplateContent = $@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure;
using MyCode.Project.Domain;
using MyCode.Project.Domain.Model;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Domain.Message;

namespace MyCode.Project.Domain.Repositories
{{
	public interface I{tablename}Repository : IRepository<{tablename}>
	{{
		
	}}
}}
";
        }
    }
}
