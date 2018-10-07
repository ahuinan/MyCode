using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Message.Response.Wechat;

namespace MyCode.Project.OutSideService
{
    /// <summary>
    /// 微信授权服务
    /// </summary>
    public interface IWeiXinAuthorizeService
    {
        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="appid">微信应用ID</param>
        /// <param name="appSecret">微信应用密匙</param>
        /// <returns></returns>
        string GetToken(string appid, string appSecret);

        /// <summary>
        /// 获取微信Ticket
        /// </summary>
        /// <param name="appid">微信应用ID</param>
        /// <param name="appSecret">微信应用密匙</param>
        /// <returns></returns>
        string GetTicket(string appid, string appSecret);

        /// <summary>
        /// 获取微信用户openid
        /// </summary>
        /// <param name="appid">微信应用ID</param>
        /// <param name="appSecret">微信应用密匙</param>
        /// <param name="code">code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。</param>
        /// <param name="grantType">授权类型</param>
        /// <returns></returns>
        string GetOpenid(string appid, string appSecret, string code, string grantType = "authorization_code");


        /// <summary>
        /// 取得微信JsSdk相关参数值
        /// </summary>
        /// <param name="appId">微信应用ID</param>
        /// <param name="ticket">JsTicket</param>
        /// <param name="url">需要授权Url地址</param>
        /// <returns></returns>
        JsSdkResp GetJsSdk(string appId, string ticket, string url);

        /// <summary>
        /// 取得token信息，该token不同于GetToken中的token
        /// </summary>
        WXAccessTokenResp GetAccessToken(string appId, string secret, string code);

		/// <summary>
		/// 取到微信用户的详细信息
		/// </summary>
	    WXUserInfoResp GetWxUserInfo(string accessToken, string openId);



    }
}
