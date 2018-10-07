using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wolf.Project.Infrastructure.Exceptions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.IO;
using System.Globalization;
using Wolf.Project.Infrastructure.Common;
using Wolf.Project.Domain.ViewModel;
using Wolf.Project.Infrastructure.Cache;
using System.Web;
using Wolf.Project.Services;
using Microsoft.Practices.Unity;

namespace Wolf.Project.WebApi
{

    public class BaseAPIController : ApiController
    {

		

		/// <summary>
		/// 当前用户ID
		/// </summary>
		public Guid UserId
		{
			get
			{
				var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
				var userid = identity.Claims.Where(c => c.Type == "UserId").Select(c => c.Value).SingleOrDefault();
				return new Guid(userid);
				
			}
		}

		public string UserName
		{
			get
			{
				var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
				var username = identity.Claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
				return username;

			}
		}

		public AccountVM CurrentUser
		{

			get
			{

				var AccountService = UnityConfig.Container.Resolve<IAccountService>();
				return AccountService.GetAccount(UserId);
				
			}
		}




		public string FileUpload(string dirPath)
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
				const string fileTypes = "gif,jpg,jpeg,png,bmp";

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
					throw new BaseException("不支持上传文件类型。");
				}
			

				string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo);

				string filePath = Path.Combine(dirPath, newFileName + fileExt);


				fileInfo.CopyTo(Path.Combine(dirTempPath, newFileName + fileExt), true);

				string domain = Request.RequestUri.Host;

				int port = Request.RequestUri.Port;


				FilePath = "http://" + domain;
				if (port > 0) {
					FilePath = FilePath + ":" + port;
				}
				FilePath = FilePath + filePath.Replace("\\", "/").Replace("~", "");


				fileInfo.Delete();


				return FilePath;
		}




	}





	
}

