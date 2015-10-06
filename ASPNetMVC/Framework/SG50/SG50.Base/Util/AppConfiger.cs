using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG50.Base.Util
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
    }
}
