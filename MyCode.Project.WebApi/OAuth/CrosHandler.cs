using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MyCode.Project.WebApi.OAuth
{
	/// <summary>
	/// CORS跨域处理器
	/// </summary>
	public class CrosHandler : DelegatingHandler
	{
		private const string Origin = "Origin";
		private const string AccessControlRequestMethod = "Access-Control-Request-Method";
		private const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
		private const string AccessControlAllowOrign = "Access-Control-Allow-Origin";
		private const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
		private const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";
		private const string AccessControlAllowCredentials = "Access-Control-Allow-Credentials";
		// <add name = "Access-Control-Allow-Headers" value="Content-Type" />
		// <add name = "Access-Control-Allow-Methods" value="GET, POST, PUT, DELETE, OPTIONS" />
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			bool isCrosRequest = request.Headers.Contains(Origin);
			bool isPrefilightRequest = request.Method == HttpMethod.Options;
			if (isCrosRequest)
			{
				Task<HttpResponseMessage> taskResult = null;
				if (isPrefilightRequest)
				{
					taskResult = Task.Factory.StartNew<HttpResponseMessage>(() =>
					{
						HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
						response.Headers.Add(AccessControlAllowOrign,
							request.Headers.GetValues(Origin).FirstOrDefault());
						string method = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
						//if (method != null)
						//{
						//    response.Headers.Add(AccessControlAllowMethods, method);
						//}
						string headers = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
						if (!string.IsNullOrWhiteSpace(headers))
						{
							response.Headers.Add(AccessControlAllowHeaders, headers);
						}
						response.Headers.Add(AccessControlAllowCredentials, "true");
						return response;
					}, cancellationToken);
				}
				else
				{
					taskResult = base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
					{
						var response = t.Result;
						response.Headers.Add(AccessControlAllowOrign,
							request.Headers.GetValues(Origin).FirstOrDefault());
						response.Headers.Add(AccessControlAllowCredentials, "true");
						return response;
					});
				}
				return taskResult;
			}
			return base.SendAsync(request, cancellationToken);
		}
	}
}