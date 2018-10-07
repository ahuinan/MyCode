using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Enumeration
{
    /// <summary>
    /// 微信消息模板类型
    /// </summary>
    public enum WechatTemplateType
    {
        /// <summary>
        /// 会员注册
        /// </summary>
        [Description("会员注册")]
        Register = 1,
        /// <summary>
        /// 会员下单
        /// </summary>
        [Description("会员下单")]
        CreateOrder = 2,
        /// <summary>
        /// 订单提交通知上级
        /// </summary>
        [Description("订单提交通知上级")]
        CommitOrder = 3,
        /// <summary>
        /// 订单审核通知
        /// </summary>
        [Description("订单审核通知")]
        AuditOrder = 4,
        /// <summary>
        /// 发货通知
        /// </summary>
        [Description("发货通知")]
        Send = 5,
        /// <summary>
        /// 充值成功通知
        /// </summary>
        [Description("充值成功通知")]
        RechargeSuccess = 6,
        /// <summary>
        /// 申请升级通知
        /// </summary>
        [Description("申请升级通知")]
        ApplyUpgrade = 7,
        /// <summary>
        /// 申请提现通知
        /// </summary>
        [Description("申请提现通知")]
        ApplyWithdraw = 8,
        /// <summary>
        /// 审核下级注册通知   
        /// </summary>
        [Description("审核下级注册通知")]
        AuditRegister = 9,
        /// <summary>
        /// 订单取消通知  
        /// </summary>]
        [Description("订单取消通知")]
        CancelOrder = 10,
        /// <summary>
        /// 审核下级充值通知 
        /// </summary>
        [Description("审核下级充值通知")]
        AuditRecharge = 11
    }
}
