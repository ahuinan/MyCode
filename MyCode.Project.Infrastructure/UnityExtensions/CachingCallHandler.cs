using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Transactions;
using System.Reflection;
using System.Web;
using MyCode.Project.Infrastructure.Cache;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;

namespace MyCode.Project.Infrastructure.UnityExtensions
{
    public class CachingCallHandler : ICallHandler
    {
        [Dependency]
        public IMyCodeCacheService Cache {
            get;
            set;
        }

        public TimeSpan ExpirationTime {
            get;set;
        }


        #region GetCacheKey(得到缓存key)
        private string GetCacheKey(MethodInfo method, IMethodInvocation input)
        {
            int hashCode = 0;

            if (input.Arguments != null && input.Arguments.Count > 0) {

                for (int i = 0; i < input.Arguments.Count; i++)
                {
                    var argument = input.Arguments[i];

                    if (argument == null)
                    {
                        hashCode = hashCode ^ (i + 1);
                    }
                    else
                    { 
                        hashCode = hashCode ^ argument.GetHashCode();
                    }
                }
            }

            return $"{method.DeclaringType.FullName}:{method.Name}:{hashCode}";
        }
        #endregion

        #region Invoke(执行)
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            MethodInfo targetMethod = (MethodInfo)input.MethodBase;
            if (targetMethod.ReturnType == typeof(void))
            {
                return getNext()(input, getNext);
            }

            string CacheKey = GetCacheKey(targetMethod,input);
            object cacheResult = Cache.Get(CacheKey) ;
            if (null == cacheResult)
            {
                IMethodReturn realReturn = getNext()(input, getNext);
                if (null == realReturn.Exception) {
                    Cache.Set(CacheKey, realReturn.ReturnValue, this.ExpirationTime);
                  
                }
                return realReturn;
            }
            return input.CreateMethodReturn(cacheResult,input.Arguments);

        }
        #endregion

        public CachingCallHandler(TimeSpan expirationtime)
        {
            this.ExpirationTime = expirationtime;
        }

        public int Order { get; set; }
    }
}
