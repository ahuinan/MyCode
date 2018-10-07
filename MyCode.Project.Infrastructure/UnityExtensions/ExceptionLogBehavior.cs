using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Reflection;
using System.ComponentModel;
using Newtonsoft;
using MyCode.Project.Infrastructure.Common;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace MyCode.Project.Infrastructure.UnityExtensions
{
    public class ExceptionLogBehavior : IInterceptionBehavior
    {

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }
        /// <summary>
        /// 通过实现此方法来拦截调用并执行所需的拦截行为。
        /// </summary>
        /// <param name="input">调用拦截目标时的输入信息。</param>
        /// <param name="getNext">通过行为链来获取下一个拦截行为的委托。</param>
        /// <returns>从拦截目标获得的返回信息。</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
        
            IMethodReturn retvalue = getNext()(input, getNext);

            #region 异常处理部分
            if (retvalue.Exception != null)
            {
                string ErrorMsg = Environment.NewLine + "-----------------------------------------" + Environment.NewLine;
                if (input.Arguments.Count > 0)
                {
                    ErrorMsg += "输入参数：" + Newtonsoft.Json.JsonConvert.SerializeObject(input.Arguments) + Environment.NewLine;
                }

                LogHelper.Error(ErrorMsg, retvalue.Exception);
            }
           
            #endregion
            return retvalue;
        }

        /// <summary>
        /// 获取一个<see cref="Boolean"/>值，该值表示当前拦截行为被调用时，是否真的需要执行
        /// 某些操作。
        /// </summary>
        public bool WillExecute
        {
            get { return true; }
        }

    }
}
 