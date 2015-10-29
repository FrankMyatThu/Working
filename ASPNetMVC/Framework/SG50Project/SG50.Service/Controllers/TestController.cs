using SG50.Base.Logging;
using SG50.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SG50.Service.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : BasedController
    {   
        [HttpPost]
        [Route("GetUserList")]
        [ValidateAntiForgeryToken(LoggerName = LoggerName)]
        [CustomizedAuthorization(LoggerName = LoggerName)]
        public IHttpActionResult GetUserList()
        {
            ApplicationLogger.WriteTrace("Start UserLogin", LoggerName);
            string ReturnString = string.Empty;
            try
            {
                ReturnString = "Returned UserList .... bla bla bla ...  " + CurrentUserID;
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }
            ApplicationLogger.WriteTrace("End UserLogin", LoggerName);
            return Ok(ReturnString);
        }
    }
}
