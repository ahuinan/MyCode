using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message.Response;
using MyCode.Project.Domain.Message.Response.Common;
using MyCode.Project.Infrastructure;

namespace MyCode.Project.OutSideService
{
    public interface IFilesUploadService
	{
		/// <summary>
		/// 根据带HTTP的图片链接将图片传到七牛空间
		/// </summary>
		FilesUploadResp UploadRemoteFile(string picUrl, string path);


	}
}
