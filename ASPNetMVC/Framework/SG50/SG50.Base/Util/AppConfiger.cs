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
        public static string UriTokenPath = ConfigurationManager.AppSettings["uri:tokenpath"].ToString();
        public static string UriLoginPath = ConfigurationManager.AppSettings["uri:loginpath"].ToString();
        public static string UrlTokenIssuer = ConfigurationManager.AppSettings["url:tokenIssuer"].ToString();
    }
}
