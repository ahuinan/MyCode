using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyCode.Project.WebApi.App_Filter
{
    /// <summary>
    /// 忽略返回结果过滤 属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,Inherited = false)]
    public class IgnoreResultHandleAttribute:Attribute
    {
    }
}