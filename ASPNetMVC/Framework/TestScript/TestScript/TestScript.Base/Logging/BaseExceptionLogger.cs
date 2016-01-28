using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestScript.Base.Logging
{
    public class BaseExceptionLogger
    {
        public static void LogError(System.Exception ex, string LoggerName)
        {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
            var stackFrame = trace.GetFrame(trace.FrameCount - 1);
            string LineNumber = stackFrame.GetFileLineNumber().ToString();
            string MethodName = stackFrame.GetMethod().Name;
            string FullName = stackFrame.GetMethod().ReflectedType.FullName;

            ApplicationLogger.WriteError(string.Format("ClassName[{0}].FunctionName[{1}].LineNo[{2}] : Exception={3}",
                                         FullName,
                                         MethodName,
                                         LineNumber,
                                         ex.Message.ToString()), LoggerName);

            if (ex.InnerException != null)
            {
                ApplicationLogger.WriteError(string.Format("ClassName[{0}].FunctionName[{1}].LineNo[{2}] : Exception Detail={3}",
                                        FullName,
                                        MethodName,
                                        LineNumber,
                                        ex.InnerException.Message.ToString()), LoggerName);
            }
        }

        public static void LogError(System.Exception ex)
        {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
            var stackFrame = trace.GetFrame(trace.FrameCount - 1);
            string LineNumber = stackFrame.GetFileLineNumber().ToString();
            string MethodName = stackFrame.GetMethod().Name;
            string FullName = stackFrame.GetMethod().ReflectedType.FullName;

            ApplicationLogger.WriteError(string.Format("ClassName[{0}].FunctionName[{1}].LineNo[{2}] : Exception={3}",
                                         FullName,
                                         MethodName,
                                         LineNumber,
                                         ex.Message.ToString()));

            if (ex.InnerException != null)
            {
                ApplicationLogger.WriteError(string.Format("ClassName[{0}].FunctionName[{1}].LineNo[{2}] : Exception Detail={3}",
                                        FullName,
                                        MethodName,
                                        LineNumber,
                                        ex.InnerException.Message.ToString()));
            }
        }
    }
}
