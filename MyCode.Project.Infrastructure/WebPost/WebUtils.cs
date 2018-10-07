using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyCode.Project.Infrastructure.Common;
using MyCode.Project.Infrastructure.Exceptions;

namespace MyCode.Project.Infrastructure.WebPost
{
    public sealed class WebUtils
    {
        private int _timeout = 2000;
        private int _readWriteTimeout = 2000;
        private bool _ignoreSSLCheck = true;

        /// <summary>
        /// 获取主机名，即域名，
        /// 范例：用户输入网址http://www.a.com/b.htm?a=1&amp;b=2，
        /// 返回值为：www.a.com
        /// </summary>
        public static string Host
        {
            get { return HttpContext.Current.Request.Url.Host; }
        }

        public static bool TrustAllValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; // 忽略SSL证书检查
        }

        /// <summary>
        /// 等待请求开始返回的超时时间
        /// </summary>
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        /// <summary>
        /// 等待读取数据完成的超时时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return this._readWriteTimeout; }
            set { this._readWriteTimeout = value; }
        }

        /// <summary>
        /// 是否忽略SSL检查
        /// </summary>
        public bool IgnoreSSLCheck
        {
            get { return this._ignoreSSLCheck; }
            set { this._ignoreSSLCheck = value; }
        }

        #region DoPost(执行HTTP POST请求)
        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> textParams)
        {
            string proxyIp = "";

            return DoPost(url, textParams, null,out proxyIp);
        }
        #endregion

        #region DoPost(执行HTTP POST请求)
        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> textParams, IDictionary<string, string> headerParams,out string proxyIp,bool useProxy = false)
        {
            HttpWebRequest req = GetWebRequest(url, "POST", headerParams,out proxyIp, useProxy);
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(textParams));
            System.IO.Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            return GetResponseAsString(rsp, encoding);
        }
        #endregion

        #region DoGet(执行HTTP GET请求)
        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="useProxy">是否适用代理</param>
        /// <returns>HTTP响应</returns>
        public  string DoGet(string url, IDictionary<string, string> textParams,out string proxyIp,bool useProxy=false)
        {
            return DoGet(url, textParams, null, out proxyIp,useProxy);
        }
        #endregion

        #region DoGet(执行HTTP GET请求)
        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> textParams, IDictionary<string, string> headerParams,out string proxyIp,bool useProxy=false)
        {
            if (textParams != null && textParams.Count > 0)
            {
                url = BuildRequestUrl(url, textParams);
            }

            HttpWebRequest req = GetWebRequest(url, "GET", headerParams, out proxyIp,useProxy);
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse rsp;

            try
            {
                rsp = req.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                ProxyIpHelper.RemoveIp(proxyIp);

                throw new BaseException($"{ex.Message}{proxyIp}");
            }
            Encoding encoding = GetResponseEncoding(rsp);
            return GetResponseAsString(rsp, encoding);
        }
        #endregion

        #region DoPost(执行带文件上传的HTTP POST请求。)
        /// <summary>
        /// 执行带文件上传的HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="textParams">请求文本参数</param>
        /// <param name="fileParams">请求文件参数</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> textParams, IDictionary<string, FileItem> fileParams, IDictionary<string, string> headerParams,out string proxyIp,bool useProxy=false)
        {
            // 如果没有文件参数，则走普通POST请求
            if (fileParams == null || fileParams.Count == 0)
            {
                return DoPost(url, textParams, headerParams,out proxyIp,useProxy);
            }

            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线

            HttpWebRequest req = GetWebRequest(url, "POST", headerParams,out proxyIp,useProxy);
            req.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;

            System.IO.Stream reqStream = req.GetRequestStream();
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            // 组装文本请求参数
            string textTemplate = "Content-Disposition:form-data;name=\"{0}\"\r\nContent-Type:text/plain\r\n\r\n{1}";
            foreach (KeyValuePair<string, string> kv in textParams)
            {
                string textEntry = string.Format(textTemplate, kv.Key, kv.Value);
                byte[] itemBytes = Encoding.UTF8.GetBytes(textEntry);
                reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                reqStream.Write(itemBytes, 0, itemBytes.Length);
            }

            // 组装文件请求参数
            string fileTemplate = "Content-Disposition:form-data;name=\"{0}\";filename=\"{1}\"\r\nContent-Type:{2}\r\n\r\n";
            foreach (KeyValuePair<string, FileItem> kv in fileParams)
            {
                string key = kv.Key;
                FileItem fileItem = kv.Value;
                if (!fileItem.IsValid())
                {
                    throw new ArgumentException("FileItem is invalid");
                }
                string fileEntry = string.Format(fileTemplate, key, fileItem.GetFileName(), fileItem.GetMimeType());
                byte[] itemBytes = Encoding.UTF8.GetBytes(fileEntry);
                reqStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                reqStream.Write(itemBytes, 0, itemBytes.Length);
                fileItem.Write(reqStream);
            }

            reqStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            return GetResponseAsString(rsp, encoding);
        }
        #endregion

        #region DoPost(执行带body体的POST请求)
        /// <summary>
        /// 执行带body体的POST请求。
        /// </summary>
        /// <param name="url">请求地址，含URL参数</param>
        /// <param name="body">请求body体字节流</param>
        /// <param name="contentType">body内容类型</param>
        /// <param name="headerParams">请求头部参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, byte[] body, string contentType, IDictionary<string, string> headerParams, out string proxyIp,bool useProxy = false)
        {

            HttpWebRequest req = GetWebRequest(url, "POST", headerParams, out proxyIp,useProxy);
            req.ContentType = contentType;
            if (body != null)
            {
                System.IO.Stream reqStream = req.GetRequestStream();
                reqStream.Write(body, 0, body.Length);
                reqStream.Close();
            }
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = GetResponseEncoding(rsp);
            return GetResponseAsString(rsp, encoding);
        }
        #endregion

        #region GetWebRequest(Get请求)
        public HttpWebRequest GetWebRequest(string url, string method, IDictionary<string, string> headerParams, out string proxyIp,bool useProxy = false)
        {
            proxyIp = "";

            HttpWebRequest req = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                if (this._ignoreSSLCheck)
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(TrustAllValidationCallback);
                }
               
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                req = (HttpWebRequest)WebRequest.Create(url);
            }

            if(useProxy)
            {
                proxyIp = ProxyIpHelper.GetProxyIp();
                WebProxy proxyObject = new WebProxy($"http://{proxyIp}/", true);
                req.ProtocolVersion = HttpVersion.Version10;
                req.Proxy = proxyObject;
            }

            if (headerParams != null && headerParams.Count > 0)
            {
                foreach (string key in headerParams.Keys)
                {
                    req.Headers.Add(key, headerParams[key]);
                }
            }
		
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            //req.UserAgent = "top-sdk-net";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
            //req.Accept = "text/xml,text/javascript";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            req.Timeout = this._timeout;
            req.ReadWriteTimeout = this._readWriteTimeout;

            return req;
        }
        #endregion

        #region GetResponseAsString(把响应流转换为文本)
        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                if (Constants.CONTENT_ENCODING_GZIP.Equals(rsp.ContentEncoding, StringComparison.OrdinalIgnoreCase))
                {
                    stream = new GZipStream(stream, CompressionMode.Decompress);
                }
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }
        #endregion

        #region BuildRequestUrl(发起一个请求)
        /// <summary>
        /// 组装含参数的请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数映射</param>
        /// <returns>带参数的请求URL</returns>
        public static string BuildRequestUrl(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                return BuildRequestUrl(url, BuildQuery(parameters));
            }
            return url;
        }
        #endregion

        #region BuildRequestUrl(发起一个请求)
        /// <summary>
        /// 组装含参数的请求URL。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="queries">一个或多个经过URL编码后的请求参数串</param>
        /// <returns>带参数的请求URL</returns>
        public static string BuildRequestUrl(string url, params string[] queries)
        {
            if (queries == null || queries.Length == 0)
            {
                return url;
            }

            StringBuilder newUrl = new StringBuilder(url);
            bool hasQuery = url.Contains("?");
            bool hasPrepend = url.EndsWith("?") || url.EndsWith("&");

            foreach (string query in queries)
            {
                if (!string.IsNullOrEmpty(query))
                {
                    if (!hasPrepend)
                    {
                        if (hasQuery)
                        {
                            newUrl.Append("&");
                        }
                        else
                        {
                            newUrl.Append("?");
                            hasQuery = true;
                        }
                    }
                    newUrl.Append(query);
                    hasPrepend = false;
                }
            }
            return newUrl.ToString();
        }
        #endregion

        #region BuildQuery(组装普通文本请求参数)
        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return null;
            }

            StringBuilder query = new StringBuilder();
            bool hasParam = false;

            foreach (KeyValuePair<string, string> kv in parameters)
            {
                string name = kv.Key;
                string value = kv.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        query.Append("&");
                    }

                    query.Append(name);
                    query.Append("=");
                    query.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    hasParam = true;
                }
            }

            return query.ToString();
        }
        #endregion

        #region GetResponseEncoding(得到字符编码)
        private Encoding GetResponseEncoding(HttpWebResponse rsp)
        {
            string charset = rsp.CharacterSet;
            if (string.IsNullOrEmpty(charset))
            {
                charset = Constants.CHARSET_UTF8;
            }
            return Encoding.GetEncoding(charset);
        }
        #endregion

        #region GetIP(获取本地IP地址)
        /// <summary>
        /// 获取本地ip地址
        /// </summary>
        /// <param name="isLocal">是否局域网ip</param>
        /// <returns></returns>
        public static string GetIP(bool isLocal = false)
        {
            string ip = string.Empty;
            if (!isLocal)
            {
                IPAddress ipAddr = Dns.Resolve(Dns.GetHostName()).AddressList[0];//获得当前IP地址
                ip = ipAddr.ToString();
            }
            else
            {
                string strUrl = "http://www.ip138.com/ip2city.asp"; //获得IP的网址了
                Uri uri = new Uri(strUrl);
                WebRequest wr = WebRequest.Create(uri);
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据
                int i = all.IndexOf("[") + 1;
                string tempip = all.Substring(i, 15);
                ip = tempip.Replace("]", "").Replace(" ", "");
            }
            return ip;
        }
        #endregion

        #region AddParam(添加Url参数)

        /// <summary>
        /// 添加Url参数
        /// </summary>
        /// <param name="url">Url地址</param>
        /// <param name="paramName">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static string AddParam(string url, string paramName, string value, bool needEncode = true)
        {
            Uri uri = new Uri(url);
            string eval = value;
            if (needEncode)
            {
                eval = HttpContext.Current.Server.UrlEncode(value);
            };

            if (!string.IsNullOrEmpty(uri.Query))
            {
                return string.Concat(url, "?" + paramName + "=" + eval);
            }
            return string.Concat(url, "&" + paramName + "=" + eval);
        }
        #endregion

        #region GetSourceUrl(取得访问的来源地址)
        /// <summary>
        /// 取得访问的来源地址
        /// </summary>
        /// <returns></returns>
        public static string GetSourceUrl()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Request.UrlReferrer.Host.ToString();
            }
        }
        #endregion

        #region GetServerIp(取得客户端的IP主机地址)
        /// <summary>
        /// 取得客户端的IP主机地址
        /// </summary>
        /// <returns></returns>
        public static string GetServerIp()
        {
            return HttpContext.Current.Request.UserHostAddress;
        }
        #endregion
    }
}
