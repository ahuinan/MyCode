using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Senparc.Weixin.CommonAPIs;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using MyCode.Project.Domain.Business.WeiXin;
using MyCode.Project.Infrastructure.Exceptions;
using MyCode.Project.Infrastructure.Extensions;

namespace MyCode.Project.OutSideService.Implementation
{
    /// <summary>
    /// 微信消息服务
    /// </summary>
    public class WeiXinMsgService:IWeiXinMsgService
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="appId">第三方用户唯一凭证</param>
        /// <param name="secrect">第三方用户唯一凭证密匙</param>
        /// <param name="data"></param>
        public void SendTemplateMsg(string appId, string secrect, TemplateModel data)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new BaseException("appid不可为空!");
            }
            if (string.IsNullOrEmpty(secrect))
            {
                throw new BaseException("secrect不可为空!");
            }

            string token = AccessTokenContainer.TryGetAccessToken(appId, secrect);

            SendTemplateMsg(token,data);
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="token">令牌,接口调用凭证</param>
        /// <param name="data">模板实体信息</param>
        public void SendTemplateMsg(string token,TemplateModel data)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new BaseException("接口调用凭证不可为空!");
            }
            if (data == null)
            {
                throw new BaseException("模板实体不可为空!");
            }
            if (string.IsNullOrEmpty(data.Openid))
            {
                throw new BaseException("接收消息的账号不能为空!");
            }
            if (string.IsNullOrEmpty(data.TemplateId))
            {
                throw new BaseException("模板ID不能为空!");
            }
            if (data.Data == null || data.Data.Count == 0)
            {
                throw new BaseException("模板数据不能为空!");
            }
            foreach (var item in data.Data)
            {
                if (string.IsNullOrEmpty(item.Code)|| string.IsNullOrEmpty(item.Value))
                {
                    throw new BaseException("模板数据不能为空!");
                }
            }

            IDictionary<string, object> dataDict = new Dictionary<string, object>();
            foreach (var item in data.Data)
            {
                dataDict.Add(item.Code, new {value = item.Value, color = item.Color});
            }

            var result = TemplateApi.SendTemplateMessage(token, data.Openid, data.TemplateId, data.Url, dataDict);
            if (result.errcode  != Senparc.Weixin.ReturnCode.请求成功)
            {
                throw new BaseException(result.errmsg);
            }                        
        }
    }
}
