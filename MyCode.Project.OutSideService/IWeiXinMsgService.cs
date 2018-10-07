using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Domain.Business.WeiXin;

namespace MyCode.Project.OutSideService
{
    /// <summary>
    /// 微信消息服务
    /// </summary>
    public interface IWeiXinMsgService
    {
        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="appId">第三方用户唯一凭证</param>
        /// <param name="secrect">第三方用户唯一凭证密匙</param>
        /// <param name="data"></param>
        void SendTemplateMsg(string appId, string secrect, TemplateModel data);

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="token">令牌,接口调用凭证</param>
        /// <param name="data">模板实体信息</param>
        void SendTemplateMsg(string token, TemplateModel data);
    }
}
