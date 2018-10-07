using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Exceptions
{
    public class BaseException : System.Exception
    {
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public BaseException()
        {
        }

        public BaseException(string _key)
            : base(_key)
        {
            this.key = _key;
        }





    }

	/// <summary>
	/// 没有实现异常
	/// </summary>

	public class NotImplementionError : BaseException
	{
		public NotImplementionError() :base("方法没有实现异常")
		{


		}
	}

    /// <summary>
    /// 工作流异常
    /// </summary>
    public class FlowError : BaseException
    {
        public FlowError(string _key)
            : base(_key)
        {
        }
    }

}
