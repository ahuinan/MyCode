using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity;
using MyCode.Project.Repositories.Common;
using MyCode.Project.Infrastructure.UnityExtensions;

namespace MyCode.Project.Repositories.Common
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionCallHandlerAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            var sqlSugarClient = container.Resolve<MyCodeSqlSugarClient>();

           // if (sqlSugarClient.Ado.Transaction != null) { return null; }

            return new TransactionCallHandler { Order = this.Order,BookSqlSugarClient = sqlSugarClient };
        }
    }
}