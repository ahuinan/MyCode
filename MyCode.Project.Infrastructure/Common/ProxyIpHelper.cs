using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.WebPost;

namespace MyCode.Project.Infrastructure.Common
{
    /// <summary>
    /// 动态代理帮助
    /// </summary>
    public class ProxyIpHelper
    {

        private static string address_xicidaili = "http://www.xicidaili.com/wn/{0}";

        private static string address_66ip = "http://www.66ip.cn/nmtq.php?getnum=20&isp=0&anonymoustype=0&start=&ports=&export=&ipaddress=&area=1&proxytype=1&api=66ip";

        private static string address_ip3366 = "http://www.ip3366.net/?stype=1&page={0}";

        private static object locker = new object(); //创建锁

        /// <summary>
        /// 存放有效的代理IP，如果无效了，则挪除
        /// </summary>
        public static List<string> proxyIps = new List<string>();

        /// <summary>
        /// 无效的IP，这样就不用每次都去比对了
        /// </summary>
        private static List<string> errProxyIps = new List<string>();

        #region RemoveIp(移除一个无效的IP)
        public static void RemoveIp(string ip)
        {
            if (!errProxyIps.Contains(ip))
            {
                errProxyIps.Add(ip);
            }
            proxyIps.Remove(ip);
            Console.WriteLine($"当前代理IP剩余：{proxyIps.Count}");
        }
        #endregion

        #region GetProxyIp(得到一个有效的IP，如果列表中有效数低于10，则少多少拿多少再放进去)
        public static string GetProxyIp()
        {
            
            

                if (proxyIps.Count > 0)
                {
                    int randNum = Utils.GetRandNum(0, proxyIps.Count - 1);

                    return proxyIps[randNum];

                }

                //GetAllCanUseProxyIp();

                var returnIp = "";

                foreach (var ip in proxyIps)
                {
                    //之前已经在错误列表了
                    if (errProxyIps.Contains(ip))
                    {
                        RemoveIp(ip);
                        continue;
                    }

                    returnIp = ip;

                    break;
                }

                return returnIp;
            
                
            
   
        }
        #endregion

        #region GetXicidailiProxy(从xicidaili.com网页上去获取代理IP，可以分页)
        /// <summary>
        /// 从xicidaili.com网页上去获取代理IP，可以分页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<string> GetXicidailiProxy(int page)
        {
            List<string> list = new List<string>();

            for (int p = 1; p <= page; p++)
            {
                string url = string.Format(address_xicidaili, p);

                WebUtils webUtil = new WebUtils();

                string proxyIp = "";

                var docText = webUtil.DoGet(url, null, out proxyIp);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(docText);

                var trNodes = doc.DocumentNode.SelectNodes("//table[@id='ip_list']")[0].SelectNodes("./tr");
                if (trNodes != null && trNodes.Count > 0)
                {
                    for (int i = 1; i < trNodes.Count; i++)
                    {
                        var tds = trNodes[i].SelectNodes("./td");
                        string ipAddress = tds[1].InnerText + ":" + int.Parse(tds[2].InnerText); ;
                        list.Add(ipAddress);
                        Console.WriteLine($"xicidaili.com得到ip:{ipAddress}");
                    }
                }
                
            }

            if (list.Count == 0) { Console.WriteLine("xicidaili.com抓取不到任何ip"); }

            return list;
        }
        #endregion

        #region GetIp3366Proxy(从ip3366.net网页上去获取代理IP，可以分页)
        /// <summary>
        /// 从ip3366.net网页上去获取代理IP，可以分页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<string> GetIp3366Proxy(int page)
        {
            List<string> list = new List<string>();
            for (int p = 1; p <= page; p++)
            {
                string url = string.Format(address_ip3366, p);

                WebUtils webUtil = new WebUtils();

                string proxyIp = "";

                var docText = webUtil.DoGet(url, null, out proxyIp);

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(docText);
                var trNodes1 = doc.DocumentNode.SelectNodes("//table")[0];
                var trNodes2 = doc.DocumentNode.SelectNodes("//table")[0].SelectSingleNode("//tbody");
                var trNodes = doc.DocumentNode.SelectNodes("//table")[0].SelectSingleNode("//tbody").SelectNodes("./tr");
                if (trNodes != null && trNodes.Count > 0)
                {
                    for (int i = 1; i < trNodes.Count; i++)
                    {
                        var tds = trNodes[i].SelectNodes("./td");
                        if (tds[3].InnerHtml == "HTTPS")
                        {
                            string ipAddress = tds[0].InnerText + ":" + int.Parse(tds[1].InnerText); ;
                            list.Add(ipAddress);
                            Console.WriteLine($"从ip3366得到ip:{ipAddress}");
                        }
                    }
                }
                   
            }

            if (list.Count == 0) { Console.WriteLine("ip3366.net抓取不到任何ip"); }

            return list;
        }
        #endregion

        #region Get66ipProxy(从66ip.cn中去获取，不需要分页)
        /// <summary>
        /// 从66ip.cn中去获取，不需要分页
        /// </summary>
        /// <returns></returns>
        public static List<string> Get66ipProxy()
        {
            Console.WriteLine("执行Get66ipProxy");

            List<string> list = new List<string>();

            WebUtils webUtil = new WebUtils();

            string proxyIp = "";

            var docText = webUtil.DoGet(address_66ip, null, out proxyIp);

            int count = 0;

            if (string.IsNullOrWhiteSpace(docText) == false)
            {
                string regex = "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\:\\d{1,5}";
                Match mstr = Regex.Match(docText, regex);
                while (mstr.Success && count < 20)
                {
                    string tempIp = mstr.Groups[0].Value;
                    Console.WriteLine($"从66ip得到ip:{tempIp}");
                    list.Add(tempIp);
                    mstr = mstr.NextMatch();
                    count++;
                }
            }

            if (list.Count == 0) { Console.WriteLine("66ip.cn抓取不到任何ip"); }

            return list;
        }
        #endregion

        #region Get89ipProxy(得到Get89ip数据，不需要分页)
        public static List<string> Get89ipProxy(int num)
        {
            Console.WriteLine("执行Get89ipProxy");

            //从接口获取10000个随机ip
            var url = $"http://www.89ip.cn/tqdl.html?api=1&num={num}";

            WebUtils webUtil = new WebUtils();

            string proxyIp = "";

            var response = webUtil.DoGet(url, null, out proxyIp);

            MatchCollection mc = Regex.Matches(response, @"(\d+).(\d+)\.(\d+)\.(\d+)\:(\d+)");

            List<string> list = new List<string>();

            foreach (Match match in mc)
            {
                Console.WriteLine($"从89ip.cn得到ip:{match.Value}");
                list.Add(match.Value);
            }

            if (list.Count == 0) { Console.WriteLine("89ip.cn抓取不到任何ip"); }

            return list;
        }
        #endregion

        #region CheckProxyIpAsync(检查IP是否可用)
        /// <summary>
        /// 检查代理IP是否可用
        /// </summary>
        /// <param name="ipAddress">ip</param>
        /// <param name="success">成功的回调</param>
        /// <param name="fail">失败的回调</param>
        /// <returns></returns>
        public static void CheckProxyIpAsync(string ipAddress)
        {
            Console.WriteLine($"开始验证Ip有效性：{ipAddress}");

            if (errProxyIps.Contains(ipAddress)) { return; }

            var methodUrl = $"https://api.douban.com/v2/book/1205054";

            string proxyIp = ipAddress;

            WebUtils webUtil = new WebUtils();

            HttpWebRequest req = null;

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(WebUtils.TrustAllValidationCallback);
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(methodUrl));

            WebProxy proxyObject = new WebProxy($"http://{ipAddress}/", true);
            req.ProtocolVersion = HttpVersion.Version10;
            req.Proxy = proxyObject;
            

            req.ServicePoint.Expect100Continue = false;
            req.Method = "GET";
            req.KeepAlive = true;
            //req.UserAgent = "top-sdk-net";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
            req.Accept = "text/xml,text/javascript";
            req.Timeout = 3000;
            req.ReadWriteTimeout = 3000;
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            HttpWebResponse rsp;

            try
            {
                rsp = req.GetResponse() as HttpWebResponse;

                if (rsp.StatusCode == HttpStatusCode.OK)
                {
                    if (!proxyIps.Contains(ipAddress))
                    {
                        proxyIps.Add(ipAddress);
                        Console.WriteLine($"新增有效ip:{ipAddress}");
                    }
                }
               
            }
            catch (Exception ex)
            {
                //移除，并加到错误列表
                ProxyIpHelper.RemoveIp(proxyIp);
                Console.WriteLine($"无效ip:{ipAddress},{ex.Message}");
            }
        }
        #endregion

        #region GetAllCanUseProxyIp(不断的去获得有效的代理IP，1分钟执行该方法)
        public static void GetAllCanUseProxyIp()
        {
            if (proxyIps.Count >= 500) { return; }

            var listAllIp = new List<string>();

            var tasks = new List<Task>();

            var cancellationTokenSource = new CancellationTokenSource();

            tasks.Add(

                Task.Factory.StartNew(() =>
                {
                    proxyIps.AddRange(Get89ipProxy(100));

                }, cancellationTokenSource.Token)
            );
            tasks.Add(

                Task.Factory.StartNew(() =>
                {
                    proxyIps.AddRange(Get66ipProxy());

                }, cancellationTokenSource.Token)
            );

            tasks.Add(

                Task.Factory.StartNew(() =>
                {
                    proxyIps.AddRange(GetIp3366Proxy(10));

                }, cancellationTokenSource.Token)
            );

            tasks.Add(

            Task.Factory.StartNew(() =>
            {
                proxyIps.AddRange(GetXicidailiProxy(10));

            }, cancellationTokenSource.Token)
            );

            try
            {
                Task.WaitAll(tasks.ToArray(), cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取代理IP有异常发生：{ex}");
                //throw ex;
            }
            


        }
        #endregion
    }

}
