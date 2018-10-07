using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.TenPayLibV3;
using MyCode.Project.Domain.Business.WeiXin;
using MyCode.Project.Infrastructure.Enumeration;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.Extensions;
using MyCode.Project.Infrastructure;
using Newtonsoft.Json;
using MyCode.Project.Domain.Message;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Xml;
using MyCode.Project.Domain.Message.Response.Wechat;
using MyCode.Project.Domain.Message.Request.Wechat;
using MyCode.Project.Infrastructure.Common;

namespace MyCode.Project.OutSideService.Implementation
{
    /// <summary>
    /// 微信支付 相关服务
    /// </summary>
    public class WeiXinPayService:IWeiXinPayService
    {
        #region GetPaySign(获取预支付交易ID签名)
        /// <summary>
        /// 获取预支付交易ID签名
        /// </summary>
        /// <param name="appId">微信APPID</param>
        /// <param name="key">微信支付——商家支付密匙</param>
        /// <param name="prepayId">预支付ID</param>
        /// <returns></returns>
        public WxPaySign GetPaySign(string appId, string key, string prepayId)
        {
            RequestHandler paysignReqHandler = new RequestHandler(null);

            //设置支付参数
            string timestamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();

            var wxPaySign = new WxPaySign()
            {
                NonceStr = nonceStr,
                TimeStamp = timestamp,
                Package = string.Format("prepay_id={0}", prepayId),
                SignType = "MD5",
				AppId = appId
            };

            paysignReqHandler.SetParameter("appId", appId);
            paysignReqHandler.SetParameter("timeStamp", wxPaySign.TimeStamp);
            paysignReqHandler.SetParameter("nonceStr", wxPaySign.NonceStr);
            paysignReqHandler.SetParameter("package", wxPaySign.Package);
            paysignReqHandler.SetParameter("signType", wxPaySign.SignType);

            string paySign = paysignReqHandler.CreateMd5Sign("key", key);
            wxPaySign.PaySign = paySign;

            return wxPaySign;
        }
        #endregion

        #region Pay(微信支付)
        /// <summary>
        /// 调用微信支付，生成预支付交易ID：prepay_id
        /// </summary>
        /// <param name="param">微信支付参数</param>
        /// <returns></returns>
        public WxPayReturn Pay(WxPayParam param)
        {
            string nonceStr = TenPayV3Util.GetNoncestr();
            string notifyUrl = string.Format(param.Domain + param.NotifyUrl);//通知地址，接收微信支付异步通知回调地址
            int totalFee = (int)(param.TotalFee * 100);
            TenPayV3Type tradeType = TenPayV3Type.JSAPI;
            switch (param.TradeType)
            {
                case TradeType.App:
                    tradeType = TenPayV3Type.APP;
                    break;
                case TradeType.JsApi:
                    tradeType = TenPayV3Type.JSAPI;
                    break;
                case TradeType.Native:
                    tradeType = TenPayV3Type.NATIVE;
                    break;
            }
            DateTime timeStart = DateTime.Now;
            DateTime timeExpire = DateTime.Now.AddHours(0.5);
            //设置参数
            TenPayV3UnifiedorderRequestData requestData = new TenPayV3UnifiedorderRequestData(
                param.AppId,
                param.MchId,
                param.Body,
                param.OutTradeNo,
                totalFee,
                param.ServiceIpAddress,
                notifyUrl,
                tradeType,
                param.Openid,
                param.Key,
                nonceStr,
                param.DeviceInfo,
                timeStart,
                timeExpire,
                param.Detail,
                param.Attach,
                param.FeeType);
            var result = TenPayV3.Unifiedorder(requestData);

            LogHelper.Info(string.Format("生成预支付ID：{0}", JsonConvert.SerializeObject(result)));

			if (!result.IsReturnCodeSuccess()) {
				throw new BaseException(result.return_msg);
			}
			if (!result.IsResultCodeSuccess()) {
				throw new BaseException(result.err_code_des);
			}
			WxPayReturn wxPayReturn = new WxPayReturn();

            wxPayReturn.ReturnCode = WxPayState.Success;
            if (param.TradeType == TradeType.Native)
            {
                wxPayReturn.CodeUrl = result.code_url;
            }
            wxPayReturn.PrepayId = result.prepay_id;
            wxPayReturn.DeviceInfo = result.device_info;
            return wxPayReturn;


        }
        #endregion

        #region ParseNotify(处理微信支付回调内容)
        /// <summary>
        /// 处理微信支付回调内容
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        public WxPayReceive ParseNotify()
        {
            ResponseHandler responseHandler = new ResponseHandler(null);
            string returnCode = responseHandler.GetParameter("return_code");
            string returnMsg = responseHandler.GetParameter("return_msg");

            LogHelper.Info("回调的XML:" + responseHandler.ParseXML());

            if (returnCode.ToUpper() != "SUCCESS")
            {
                throw new BaseException(string.Format("微信支付回调，出现异常：{0}", returnMsg));
            }
            return new WxPayReceive()
            {
                IsSuccess = true,
                Attach = responseHandler.GetParameter("attach"),
                Openid = responseHandler.GetParameter("openid"),
                OutTradeNo = responseHandler.GetParameter("out_trade_no"),
                TimeEnd = DateTime.ParseExact(responseHandler.GetParameter("time_end"), "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture),
                TransactionId = responseHandler.GetParameter("transaction_id"),
				TotalFee = decimal.Parse(responseHandler.GetParameter("total_fee"))/100
			};
        }
		#endregion

		#region SendRedPackage(微信发红包)
		public void SendRedPackage(RedPackageRequest request)
		{
			string nonceStr = TenPayV3Util.GetNoncestr();

			string paySign;//签名

			if (string.IsNullOrEmpty(request.OpenId)) {
				throw new BaseException("openId为空");
			}

			RequestHandler packageReqHandler = new RequestHandler();
			//设置package订单参数
			packageReqHandler.SetParameter("nonce_str", nonceStr);              //随机字符串
			packageReqHandler.SetParameter("wxappid", request.AppId);         //公众账号ID
			packageReqHandler.SetParameter("mch_id", request.MchId);          //商户号
			packageReqHandler.SetParameter("mch_billno", request.BillNo);                 //填入商家订单号
			packageReqHandler.SetParameter("send_name", request.SendFrom);                //红包发送者名称
			packageReqHandler.SetParameter("re_openid", request.OpenId);                 //接受收红包的用户的openId
			packageReqHandler.SetParameter("total_amount", Convert.ToInt32(request.Money * 100m).ToString());                //付款金额，单位分
			packageReqHandler.SetParameter("total_num", "1");               //红包发放总人数
			packageReqHandler.SetParameter("wishing", request.RedPackageWish);               //红包祝福语
			packageReqHandler.SetParameter("client_ip", request.UserHostIp);               //调用接口的机器Ip地址
			packageReqHandler.SetParameter("act_name", request.ActivityName);   //活动名称
			packageReqHandler.SetParameter("remark", request.Note);   //备注信息

			paySign = packageReqHandler.CreateMd5Sign("key", request.PayKey);
			packageReqHandler.SetParameter("sign", paySign);

			//发红包需要post的数据
			string data = packageReqHandler.ParseXML();

			//发红包接口地址
			string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
		
			string cert = request.CertificatePath;

			string password = request.CertificatePwd;

			//调用证书
			X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
			ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

			#region 发起post请求
			HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
			webrequest.ClientCertificates.Add(cer);
			webrequest.Method = "post";


			byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
			webrequest.ContentLength = postdatabyte.Length;
			Stream stream = webrequest.GetRequestStream();
			stream.Write(postdatabyte, 0, postdatabyte.Length);
			stream.Close();

			HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string responseContent = streamReader.ReadToEnd();
            #endregion

            LogHelper.Info(string.Format("发红包结果：{0}", JsonConvert.SerializeObject(responseContent)));

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(responseContent);

			NormalRedPackResult normalReturn = new NormalRedPackResult
			{
				err_code = "",
				err_code_des = ""
			};

			#region 对返回结果进行处理
			if (doc.SelectSingleNode("/xml/return_code") != null)
			{
				normalReturn.return_code = doc.SelectSingleNode("/xml/return_code").InnerText;
			}
			if (doc.SelectSingleNode("/xml/return_msg") != null)
			{
				normalReturn.return_msg = doc.SelectSingleNode("/xml/return_msg").InnerText;
			}

			if (normalReturn.ReturnCodeSuccess)
			{
				//redReturn.sign = doc.SelectSingleNode("/xml/sign").InnerText;
				if (doc.SelectSingleNode("/xml/result_code") != null)
				{
					normalReturn.result_code = doc.SelectSingleNode("/xml/result_code").InnerText;
				}

				if (normalReturn.ResultCodeSuccess)
				{
					if (doc.SelectSingleNode("/xml/mch_billno") != null)
					{
						normalReturn.mch_billno = doc.SelectSingleNode("/xml/mch_billno").InnerText;
					}
					if (doc.SelectSingleNode("/xml/mch_id") != null)
					{
						normalReturn.mch_id = doc.SelectSingleNode("/xml/mch_id").InnerText;
					}
					if (doc.SelectSingleNode("/xml/wxappid") != null)
					{
						normalReturn.wxappid = doc.SelectSingleNode("/xml/wxappid").InnerText;
					}
					if (doc.SelectSingleNode("/xml/re_openid") != null)
					{
						normalReturn.re_openid = doc.SelectSingleNode("/xml/re_openid").InnerText;
					}
					if (doc.SelectSingleNode("/xml/total_amount") != null)
					{
						normalReturn.total_amount = doc.SelectSingleNode("/xml/total_amount").InnerText;
					}

					
				}
				else
				{
					if (doc.SelectSingleNode("/xml/err_code") != null)
					{
						normalReturn.err_code = doc.SelectSingleNode("/xml/err_code").InnerText;
					}
					if (doc.SelectSingleNode("/xml/err_code_des") != null)
					{
						normalReturn.err_code_des = doc.SelectSingleNode("/xml/err_code_des").InnerText;
					}
					if (doc.SelectSingleNode("/xml/mch_billno") != null)
					{
						normalReturn.mch_billno = doc.SelectSingleNode("/xml/mch_billno").InnerText;
					}
					if (doc.SelectSingleNode("/xml/mch_id") != null)
					{
						normalReturn.mch_id = doc.SelectSingleNode("/xml/mch_id").InnerText;
					}
					if (doc.SelectSingleNode("/xml/wxappid") != null)
					{
						normalReturn.wxappid = doc.SelectSingleNode("/xml/wxappid").InnerText;
					}
					if (doc.SelectSingleNode("/xml/re_openid") != null)
					{
						normalReturn.re_openid = doc.SelectSingleNode("/xml/re_openid").InnerText;
					}
					if (doc.SelectSingleNode("/xml/total_amount") != null)
					{
						normalReturn.total_amount = doc.SelectSingleNode("/xml/total_amount").InnerText;
					}

				}
			}
			#endregion

			//var result = RedPackApi.SendNormalRedPack(request.AppId,
			//	request.MchId,
			//	request.PayKey,
			//	//request.CertificatePath,
			//	"E:\\pro\\api\\uploadfiles\\20170814114751_2554.p12",
			//	request.OpenId,
			//	request.SendFrom,
			//	request.UserHostIp,
			//	Convert.ToInt32(request.Money * 100m),
			//	request.RedPackageWish,
			//	request.ActivityName,
			//	request.Note,
			//	out nonceStr,
			//	out paySign,
			//	request.BillNo,
			//	null,
			//	null
			//	);

			if (!normalReturn.ReturnCodeSuccess)
			{
				throw new BaseException(normalReturn.return_msg);
			}
			if (!normalReturn.ResultCodeSuccess)
			{
				throw new BaseException(normalReturn.err_code_des);
			}

		}
		#endregion

		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			if (errors == SslPolicyErrors.None)
				return true;
			return false;
		}

		#region GetHBInfo(获取红包状态信息)
		public RedPackageStatusResponse GetHBInfo(GetRedPackageRequest request) {

			string nonceStr = TenPayV3Util.GetNoncestr();
			RequestHandler packageReqHandler = new RequestHandler(null);
			packageReqHandler.SetParameter("nonce_str", nonceStr);             
			packageReqHandler.SetParameter("appid", request.AppId);        
			packageReqHandler.SetParameter("mch_id", request.MchId);         
			packageReqHandler.SetParameter("mch_billno", request.BillNo);                
			packageReqHandler.SetParameter("bill_type", "MCHT");              
			string sign = packageReqHandler.CreateMd5Sign("key", request.PayKey);
			packageReqHandler.SetParameter("sign", sign);                     
																				
			string data = packageReqHandler.ParseXML();

			//发红包接口地址
			string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/gethbinfo";
			//本地或者服务器的证书位置（证书在微信支付申请成功发来的通知邮件中）
			string cert = request.CertificatePath;
			//私钥（在安装证书时设置）
			string password = request.CertificatePwd;
			ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

			//调用证书
			X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);

			#region 发起post请求
			HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
			webrequest.ClientCertificates.Add(cer);
			webrequest.Method = "post";

			byte[] postdatabyte = Encoding.UTF8.GetBytes(data);
			webrequest.ContentLength = postdatabyte.Length;
			Stream stream;
			stream = webrequest.GetRequestStream();
			stream.Write(postdatabyte, 0, postdatabyte.Length);
			stream.Close();

			HttpWebResponse httpWebResponse = (HttpWebResponse)webrequest.GetResponse();
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			string responseContent = streamReader.ReadToEnd();

            LogHelper.Info(string.Format("发红包结果：{0}", responseContent));

			if (string.IsNullOrEmpty(responseContent)) {
				throw new BaseException("获取红包状态返回值为空");
			}
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(responseContent);

			#region 返回的查询结果集
			RedPackageStatusResponse searchReturn = new RedPackageStatusResponse
			{
				err_code = "",
				err_code_des = ""
			};
			if (doc.SelectSingleNode("/xml/return_code") != null)
			{
				searchReturn.return_code = doc.SelectSingleNode("/xml/return_code").InnerText.ToUpper();
			}
			if (doc.SelectSingleNode("/xml/return_msg") != null)
			{
				searchReturn.return_msg = doc.SelectSingleNode("/xml/return_msg").InnerText;
			}

			if (searchReturn.return_code == "SUCCESS")
			{
				//redReturn.sign = doc.SelectSingleNode("/xml/sign").InnerText;
				if (doc.SelectSingleNode("/xml/result_code") != null)
				{
					searchReturn.result_code = doc.SelectSingleNode("/xml/result_code").InnerText.ToUpper();
				}

				if (searchReturn.result_code == "SUCCESS")
				{
					if (doc.SelectSingleNode("/xml/mch_billno") != null)
					{
						searchReturn.mch_billno = doc.SelectSingleNode("/xml/mch_billno").InnerText;
					}
					
					if (doc.SelectSingleNode("/xml/detail_id") != null)
					{
						searchReturn.detail_id = doc.SelectSingleNode("/xml/detail_id").InnerText;
					}
					if (doc.SelectSingleNode("/xml/status") != null)
					{
						searchReturn.status = doc.SelectSingleNode("/xml/status").InnerText;
					}
				

					if (doc.SelectSingleNode("/xml/reason") != null)
					{
						searchReturn.reason = doc.SelectSingleNode("/xml/reason").InnerText;
					}

				}
				else
				{
					if (doc.SelectSingleNode("/xml/err_code") != null)
					{
						searchReturn.err_code = doc.SelectSingleNode("/xml/err_code").InnerText;
					}
					if (doc.SelectSingleNode("/xml/err_code_des") != null)
					{
						searchReturn.err_code_des = doc.SelectSingleNode("/xml/err_code_des").InnerText;
					}
				}
				#endregion
			}

			if (searchReturn.return_code == "FAIL") {
				throw new BaseException(searchReturn.return_msg);
			}
			if (searchReturn.result_code == "FAIL") {
				throw new BaseException(string.Format("错误代码：{0}，错误代码描述：{1}", searchReturn.err_code, searchReturn.err_code_des));
			}
			return searchReturn;
			#endregion
		}
		#endregion
	}
}
