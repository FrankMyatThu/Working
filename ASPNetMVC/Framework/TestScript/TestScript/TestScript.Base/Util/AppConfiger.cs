using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestScript.Base.Util
{
    public class AppConfiger
    {
        public static string CorsOrigins = ConfigurationManager.AppSettings["cors:Origins"].ToString();
        public static string CorsHeaders = ConfigurationManager.AppSettings["cors:Headers"].ToString();
        public static string CorsMethods = ConfigurationManager.AppSettings["cors:Methods"].ToString();
        public static string UrlTokenIssuer = ConfigurationManager.AppSettings["url:tokenIssuer"].ToString();
        public static string IsWriteTraceOn = ConfigurationManager.AppSettings["IsWriteTraceOn"].ToString();
        public static string IsWriteInfoOn = ConfigurationManager.AppSettings["IsWriteInfoOn"].ToString();
        public static string IsWriteWarningOn = ConfigurationManager.AppSettings["IsWriteWarningOn"].ToString();
        public static string IsWriteErrorOn = ConfigurationManager.AppSettings["IsWriteErrorOn"].ToString();
        public static string ApplicationTokenLifeTime = ConfigurationManager.AppSettings["ApplicationTokenLifeTime"].ToString();
        public static string LoginNotificationMessage = ConfigurationManager.AppSettings["LoginNotificationMessage"].ToString();
        public static string DefaultFromEmailAddress = ConfigurationManager.AppSettings["DefaultFromEmailAddress"].ToString();
        public static string SMTP_HostName = ConfigurationManager.AppSettings["SMTP_HostName"].ToString();
        public static string SMTP_Port = ConfigurationManager.AppSettings["SMTP_Port"].ToString();
        public static string SMTP_EnableSSL = ConfigurationManager.AppSettings["SMTP_EnableSSL"].ToString();
        public static string SMTP_UserName = ConfigurationManager.AppSettings["SMTP_UserName"].ToString();
        public static string SMTP_Password = ConfigurationManager.AppSettings["SMTP_Password"].ToString();
    }
}