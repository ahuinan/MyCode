using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Generate.Template;
using MyCode.Project.Repositories.Common;

namespace MyCode.Project.GenerateCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(FileHelper.MapPath("/"));

            using (var db = new MyCodeSqlSugarClient())
            {
                var tables = db.DbMaintenance.GetTableInfoList();

                var solutionPath = FileUtils.GetSolutionPath();

                Console.WriteLine($"当前解决方案所在目录：{solutionPath}");

                //生成所有实体
                db.DbFirst.IsCreateAttribute(true).CreateClassFile(Path.Combine(FileUtils.GetSolutionPath(), "MyCode.Project.Domain", "Model"), "MyCode.Project.Domain.Model");

                foreach (var table in tables)
                {

                    Console.WriteLine(table.Name);

                    //创建仓储接口
                    var templateForRepositoryInterface = new TemplateForRepositoryInterface(table.Name);
                    templateForRepositoryInterface.CreateFile();

                    //创建仓储实现类
                    var templateForRepository = new TemplateForRepository(table.Name);
                    templateForRepository.CreateFile();

                }

                Console.WriteLine("代码生成成功");
            }


            Console.ReadKey();
        }
    }
}
