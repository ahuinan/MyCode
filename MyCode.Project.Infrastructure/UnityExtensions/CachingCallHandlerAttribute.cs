using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity;


namespace MyCode.Project.Infrastructure.UnityExtensions
{
    [AttributeUsage(AttributeTargets.Method)]

    public class CachingCallHandlerAttribute : HandlerAttribute
    {
        public TimeSpan ExpirationTime;
        public CachingCallHandlerAttribute(string expiretionTime = "")
        {
            if (!string.IsNullOrEmpty(expiretionTime))
            {
                TimeSpan expirationTimeSpan;
                if (!TimeSpan.TryParse(expiretionTime, out expirationTimeSpan)) {
                    throw new ArgumentException("输入的过期时间格式不正确");
                }
                this.ExpirationTime = expirationTimeSpan;
            }
        }

        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            //return container.Resolve<CachingCallHandler>();
            return container.Resolve<CachingCallHandler>(new ParameterOverride("expirationtime", this.ExpirationTime));
            //return  new CachingCallHandler(this.ExpirationTime) { Order = this.Order };

           // return new CachingCallHandler(this.ExpirationTime.GetValueOrDefault()) { Order = this.Order };
        }
    }
}
