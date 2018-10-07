using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.WebPost;
using Newtonsoft;
using Newtonsoft.Json;
using MyCode.Project.Domain.Config;
using MyCode.Project.Domain.Message.Response.Common;

namespace MyCode.Project.OutSideService.Implementation
{
    /// <summary>
    /// 微信授权服务
    /// </summary>
    public class FilesUploadService : IFilesUploadService
    {

		public FilesUploadService()
		{
		}


		#region UploadRemoteFile(根据URL将图片上传到七牛空间)
		public FilesUploadResp UploadRemoteFile(string picUrl, string path) {

			var obj = new {
				fileUrl = picUrl,
				key = path
			};

			WebUtils webUtils = new WebUtils();

            string proxyIp = "";
			IDictionary<string, string> dic = new Dictionary<string, string>();
			//dic.Add("Accept", "application/json");
			var result = webUtils.DoPost(SystemConfig.PictureUrl + "/api/Qiniu/UploadRemoteFile", 
				System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(obj)), 
				"application/json", 
				dic,
                out proxyIp);

			var returnResult =  JsonConvert.DeserializeObject<FilesUploadResp>(result);

			if (returnResult.code != 0) {
				throw new BaseException(returnResult.message);
			}

			return returnResult;

		}
		#endregion
	}
}
