using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using MyCode.Project.Infrastructure.Common;
using System.Data.Common;
using MyCode.Project.Repositories.Common;

namespace MyCode.Project.Repositories.Common
{
	public class TransactionCallHandler : ICallHandler
	{


		public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
		{

            if (BookSqlSugarClient.Ado.Transaction != null) {

                IMethodReturn method = getNext()(input, getNext);

                if (method.Exception != null)
            	{
            		return input.CreateExceptionMethodReturn(method.Exception);
            	}

                return method;
            }

            BookSqlSugarClient.Ado.BeginTran();

                IMethodReturn methodReturn = getNext()(input, getNext);

                if (methodReturn.Exception != null) {

                    BookSqlSugarClient.Ado.RollbackTran();

                    return input.CreateExceptionMethodReturn(methodReturn.Exception);
                }

                BookSqlSugarClient.Ado.CommitTran();

                return methodReturn;
        }

		public int Order { get; set; }

        public MyCodeSqlSugarClient BookSqlSugarClient { get; set; }
    }
}
