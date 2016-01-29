using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestScript.Base.Logging;

namespace TestScript.Service.Controllers
{
    public class BasedController : ApiController
    {
        public const string LoggerName = "TestScript_Appender_Logger";
        public const string Key_ModelStateInvalidError = "Key_ModelStateInvalidError";

        public string CurrentUserID
        {
            get
            {
                string ReturnValue = string.Empty;
                try
                {
                    string EncodedJwtToken = Request.Headers.Authorization.Parameter;
                    //ReturnValue = (new AuthTokenChecker()).GetAudienceId(EncodedJwtToken);
                }
                catch (Exception ex)
                {
                    BaseExceptionLogger.LogError(ex, LoggerName);
                }
                return ReturnValue;
            }
        }
    }
}
