using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Senparc.Weixin;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP.Containers;
using MyCode.Project.Infrastructure;
using MyCode.Project.Infrastructure.Exceptions;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.TenPayLib;
using Senparc.Weixin.MP.TenPayLibV3;
using MyCode.Project.Domain.Message.Response.Wechat;
using MyCode.Project.Infrastructure.Common;

namespace MyCode.Project.OutSideService.Implementation
{
    /// <summary>
    /// 微信授权服务
    /// </summary>
    public class WeiXinAuthorizeService:IWeiXinAuthorizeService
    {
        #region GetToken(获取微信Token)
        /// <summary>
        /// 获取微信Token
        /// </summary>
        /// <param name="appid">微信应用ID</param>
        /// <param name="appSecret">微信应用密匙</param>
        /// <returns></returns>
        public string GetToken(string appid, string appSecret)
        {
            return AccessTokenContainer.TryGetAccessToken(appid, appSecret);
        }
        #endregion

        #region GetTicket(获取微信Ticket)
        /// <summary>
        /// 获取微信Ticket
        /// </summary>
        /// <param name="appid">微信应用ID</param>
        /// <param name="appSecret">微信应用密匙</param>
        /// <returns></returns>
        public string GetTicket(string appid, string appSecret)
        {
            return JsApiTicketContainer.TryGetJsApiTicket(appid, appSecret,true);
        }
        #endregion

        #region GetOpenid(获取微信用户openid)
        /// <summary>
        /// 获取微信用户openid
        /// </summary>
        /// <param name="appid">微信应用ID</param>
        /// <param name="appSecret">微信应用密匙</param>
        /// <param name="code">code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。</param>
        /// <param name="grantType">授权类型</param>
        /// <returns></returns>
        public string GetOpenid(string appid, string appSecret, string code, string grantType = "authorization_code")
        {
            var result = OAuthApi.GetAccessToken(appid, appSecret, code, grantType);
            if (result == null)
            {
                LogHelper.Error("获取微信openid失败!");
                return string.Empty;
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                LogHelper.Error("获取微信openid失败!");
                return string.Empty;
            }
            return result.openid;
        }
		#endregion

		#region GetAccessToken(取得access_token和openId)
		public WXAccessTokenResp GetAccessToken(string appId, string secret, string code)
	    {
		    var result = OAuthApi.GetAccessToken(appId, secret, code);
		    if (result.errcode != ReturnCode.请求成功)
		    {
			    throw new BaseException("WeiXinAuthorizeService.GetAccessToken请求失败");
		    }
		    return new WXAccessTokenResp()
		    {
				AccessToken = result.access_token,
				OpenId = result.openid
			};

	    }
		#endregion

		#region GetJsSdk(取得微信JsSdk相关参数值)
		/// <summary>
		/// 取得微信JsSdk相关参数值
		/// </summary>
		/// <param name="appId">微信应用ID</param>
		/// <param name="ticket">JsTicket</param>
		/// <param name="url">需要授权Url地址</param>
		/// <returns></returns>
		public JsSdkResp GetJsSdk(string appId,string ticket,string url)
        {
            var decodeUrl = HttpUtility.UrlDecode(url);
            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();

            string signature = "";
            Senparc.Weixin.MP.TenPayLib.RequestHandler paySignReqHandler=new Senparc.Weixin.MP.TenPayLib.RequestHandler(null);

            paySignReqHandler.SetParameter("jsapi_ticket",ticket);
            paySignReqHandler.SetParameter("noncestr",nonceStr);
            paySignReqHandler.SetParameter("timestamp",timeStamp);
            paySignReqHandler.SetParameter("url", decodeUrl);

            signature = paySignReqHandler.CreateSHA1Sign();

            return new JsSdkResp
            {
                AppId = appId,
                Timestamp = timeStamp,
                NonceStr = nonceStr,
                Signature = signature
            };

        }
		#endregion

		#region  GetWxUserInfo(获取微信用户详细信息)
		public WXUserInfoResp GetWxUserInfo(string accessToken,string openId)
	    {
		    var result =  OAuthApi.GetUserInfo(accessToken, openId);

			//图像只要小图，不要用大图
		    var headImg = "";
		    if (!string.IsNullOrEmpty(result.headimgurl))
		    {
			    headImg = result.headimgurl;
			    if (headImg.Substring(headImg.Length - 1, 1) == "0")
			    {
				    headImg = headImg.Substring(0, headImg.Length - 1) + "132";
			    }
		    }
		    return new WXUserInfoResp()
		    {
			    OpenId = result.openid,
			    NickName = result.nickname,
			    Sex = result.sex,
			    Province = result.province,
			    City = result.city,
			    Country = result.country,
			    HeadImgUrl = headImg

			};
	    }
		#endregion
	}
}
