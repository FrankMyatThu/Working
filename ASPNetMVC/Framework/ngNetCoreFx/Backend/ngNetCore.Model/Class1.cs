using ngNetCore.Base.Logging;
using System;
using System.Reflection;

namespace ngNetCore.Model
{
    public class Class1
    {
        public Class1()
        {
            ApplicationLogger.WriteInfo(string.Format("[{0}.{1}]", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name), "CustomizedLogger");
        }

        public void TestFunction()
        {
            ApplicationLogger.WriteInfo(string.Format("[{0}.{1}]", MethodBase.GetCurrentMethod().DeclaringType.FullName, MethodBase.GetCurrentMethod().Name), "CustomizedLogger");
        }
    }
}
