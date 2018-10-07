using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCode.Project.Generate.Template
{
    public class BaseTemplate
    {
        /// <summary>
        /// 表名
        /// </summary>
        private string _tableName;

        /// <summary>
        /// 要保存的路径
        /// </summary>
        protected string SavePath { get; set; }


        /// <summary>
        /// 模板内容
        /// </summary>
        protected string TemplateContent { get; set; }


        public BaseTemplate(string tableName)
        {
            _tableName = tableName;

          
        }

        /// <summary>
        /// 生成文件
        /// </summary>
        public void CreateFile() {

            if (!FileUtils.IsFileExists(SavePath))
            {
                FileUtils.CreateFile(SavePath, TemplateContent);
            }
        }




    }
}
