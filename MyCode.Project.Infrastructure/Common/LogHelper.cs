using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCode.Project.Infrastructure.Common
{
    public class LogHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("MyCode.Logger");

        #region Error(错误记录)
        public static void Error(string message, Exception ex)
        {
            log.Error(message, ex);
        }
        #endregion

        #region Error(错误记录)
        public static void Error(object obj)
        {
            log.Error(obj + Environment.NewLine);
        }
        #endregion

        #region Info(信息日志)
        public static void Info(string message)
        {
            log.Info(message + Environment.NewLine);
        }
        #endregion

        #region Info(信息日志)
        public static void Info(object obj)
        {
            log.Info(obj);
        }
        #endregion
    }
}
