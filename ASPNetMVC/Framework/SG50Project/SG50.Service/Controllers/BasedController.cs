using SG50.Base.Logging;
using SG50.Model.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SG50.Service.Controllers
{
    public class BasedController : ApiController
    {
        public const string LoggerName = "SG50Project_Appender_Logger";
        public const string Key_ModelStateInvalidError = "Key_ModelStateInvalidError";
        
        public string CurrentUserID {
            get
            {
                string ReturnValue = string.Empty;
                try
                {
                    string EncodedJwtToken = Request.Headers.Authorization.Parameter;
                    ReturnValue = (new AuthTokenChecker()).GetAudienceId(EncodedJwtToken);                
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
