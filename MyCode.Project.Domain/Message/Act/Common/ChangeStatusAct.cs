using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyCode.Project.Infrastructure.Exceptions;

namespace MyCode.Project.Domain.Message.Act.Common
{
    public class ChangeStatusAct
    {
        /// <summary>
		/// 传入Guid格式的一组主键ID
		/// </summary>
		public List<Guid> ListId
        {
            get;
            set;
        }

        /// <summary>
        /// 操作人，可以不传
        /// </summary>
        public string Editor
        {
            get;
            set;
        }

        /// <summary>
        /// 状态1=启用，申请通过 0=禁用，申请拒绝
        /// </summary>
        public int Status
        {

            get;
            set;
        }

        /// <summary>
        /// 检查状态
        /// </summary>
	    public void CheckStatus()
        {
            if (Status < 0 || Status > 1) { throw new BaseException("状态值错误");}

            if (ListId == null || ListId.Count == 0) { throw new BaseException("没有选择项"); }
        }
    }
}
