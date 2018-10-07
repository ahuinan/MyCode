using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyCode.Project.Infrastructure.Common
{
    public class UploadFile
    {

        public static string Upload(HttpPostedFile file, string uploadPath)
        {
            try
            {
                var path = HttpContext.Current.Server.MapPath(uploadPath + file.FileName);
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(uploadPath)))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                file.SaveAs(path);
                return path;
            }
            catch
            {
                return "error";
            }

        }

    }
}
