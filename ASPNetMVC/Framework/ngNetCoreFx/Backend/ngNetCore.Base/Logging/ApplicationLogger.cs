using log4net.Repository;
using ngNetCore.Base.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ngNetCore.Base.Logging
{
    public class ApplicationLogger
    {
        private static readonly ILoggerRepository _loggerRepository = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void WriteError(string strMessage)
        {
            if (Convert.ToBoolean(AppConfiger.IsWriteErrorOn))
            {
                log.Error(strMessage);
            }
        }

        public static void WriteWarning(string strMessage)
        {
            if (Convert.ToBoolean(AppConfiger.IsWriteWarningOn))
            {
                log.Warn(strMessage);
            }
        }

        public static void WriteInfo(string strMessage)
        {
            if (Convert.ToBoolean(AppConfiger.IsWriteInfoOn))
            {
                log.Info(strMessage);
            }
        }

        public static void WriteTrace(string strMessage)
        {
            if (Convert.ToBoolean(AppConfiger.IsWriteTraceOn))
            {
                log.Debug(strMessage);
            }
        }

        #region Log with Logger
        public static void WriteError(string strMessage, string loggerName)
        {
            if (Convert.ToBoolean(AppConfiger.IsWriteErrorOn))
            {
                log4net.ILog customLog = log4net.LogManager.GetLogger(_loggerRepository.Name, loggerName);
                customLog.Error(strMessage);
            }
        }
        public static void WriteWarning(string strMessage, string loggerName)
        {
            if (Convert.ToBoolean(AppConfiger.IsWriteWarningOn))
            {
                log4net.ILog customLog = log4net.LogManager.GetLogger(_loggerRepository.Name, loggerName);
                customLog.Warn(strMessage);
            }
        }
        public static void WriteInfo(string strMessage, string loggerName)
        {
            if (Convert.ToBoolean(AppConfiger.IsWriteInfoOn))
            {
                log4net.ILog customLog = log4net.LogManager.GetLogger(_loggerRepository.Name, loggerName);
                customLog.Info(strMessage);
            }
        }
        public static void WriteTrace(string strMessage, string loggerName)
        {
            if (Convert.ToBoolean(AppConfiger.IsWriteTraceOn))
            {
                log4net.ILog customLog = log4net.LogManager.GetLogger(_loggerRepository.Name, loggerName);
                customLog.Debug(strMessage);
            }
        }
        #endregion Log with Logger
    }
}
