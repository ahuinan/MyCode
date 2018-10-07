using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MyCode.Project.Domain.Business.WeiXin;
using MyCode.Project.Domain.Message;
using MyCode.Project.Domain.Message.Request.Wechat;
using MyCode.Project.Domain.Message.Response.Wechat;

namespace MyCode.Project.OutSideService
{
    /// <summary>
    /// 微信支付 相关服务
    /// </summary>
    public interface IWeiXinPayService
    {
        /// <summary>
        /// 获取预支付交易ID签名
        /// </summary>
        /// <param name="appId">微信APPID</param>
        /// <param name="key">微信支付——商家支付密匙</param>
        /// <param name="prepayId">预支付ID</param>
        /// <returns></returns>
        WxPaySign GetPaySign(string appId, string key, string prepayId);

        /// <summary>
        /// 调用微信支付，生成预支付交易ID：prepay_id
        /// </summary>
        /// <param name="param">微信支付参数</param>
        /// <returns></returns>
        WxPayReturn Pay(WxPayParam param);

		/// <summary>
		/// 处理微信支付回调内容
		/// </summary>
		/// <param name="context">Http上下文</param>
		/// <returns></returns>
		WxPayReceive ParseNotify();

		/// <summary>
		/// 微信发红包
		/// </summary>
		/// <param name="request"></param>
		void SendRedPackage(RedPackageRequest request);

		/// <summary>
		/// 微信发红包
		/// </summary>
		/// <param name="request"></param>
		RedPackageStatusResponse GetHBInfo(GetRedPackageRequest request);

	}
}
