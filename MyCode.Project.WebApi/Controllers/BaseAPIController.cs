using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyCode.Project.Infrastructure.Exceptions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.IO;
using System.Globalization;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Cache;
using System.Web;
using MyCode.Project.Services;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using MyCode.Project.Infrastructure.WebPost;
using MyCode.Project.Domain.Message.Response.User;
using MyCode.Project.Infrastructure.Constant;
using Newtonsoft.Json.Linq;

namespace MyCode.Project.WebApi.Controllers
{

    public class BaseAPIController : ApiController
    {

        /// <summary>
        /// 取得登陆信息
        /// </summary>
        protected LoginInfo CurrentLogin
        {
            get
            {
                var obj = this.RequestContext.RouteData.Values[Const.LoginInfoKey];

                return ((JObject)obj).ToObject<LoginInfo>();
            }
        }

        /// <summary>
        /// 得到一个HttpContextBase队形
        /// </summary>
        public HttpContextBase CurrentHttpContext
        {
            get
            {
                return (System.Web.HttpContextBase)Request.Properties["MS_HttpContext"];
            }
        }

        #region FileUpload(文件上传)
        protected string FileUpload(string dirPath)
		{
			if (!Request.Content.IsMimeMultipartContent("form-data"))
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}
			string dirTempPath = HttpContext.Current.Server.MapPath(dirPath);

			if (!Directory.Exists(dirTempPath))
			{
				Directory.CreateDirectory(dirTempPath);
			}
			//设置上传目录
			var provider = new MultipartFormDataStreamProvider(dirTempPath);

			Task.Run(async () => await Request.Content.ReadAsMultipartAsync(provider)).Wait();

			
				var file = provider.FileData[0];

		

				//最大文件大小
				const int maxSize = 10000000;
				//定义允许上传的文件扩展名
				const string fileTypes = "gif,jpg,jpeg,png,bmp,xls,xlsx,rar,zip,p12";

				string oldFileName = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');

				string FilePath = "";

				var fileInfo = new FileInfo(file.LocalFileName);


				if (fileInfo.Length <= 0)
				{
					throw new BaseException("请选择上传文件。");
				}


				if (fileInfo.Length > maxSize)
				{
					throw new BaseException("上传文件大小超过限制。");
				}
				
				var fileExt = oldFileName.Substring(oldFileName.LastIndexOf('.'));

				if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
				{
					throw new BaseException("不支持上传文件类型，只支持"+ fileTypes);
				}
			

				string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);

			string filePath = Path.Combine(dirPath, newFileName + fileExt).Replace("~", "").Replace("\\", "/");

				FilePath = Path.Combine(dirTempPath, newFileName + fileExt);

				fileInfo.CopyTo(FilePath, true);


				fileInfo.Delete();

	
				return filePath;
		}
        #endregion




    }





	
}

