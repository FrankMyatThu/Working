using SG50.Base.Logging;
using SG50.Model.BusinessLogic;
using SG50.Model.ViewModel;
using SG50.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SG50.Service.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {
        string Key_ModelStateInvalidError = "Key_ModelStateInvalidError";
        const string LoggerName = "SG50Project_Appender_Logger";

        [HttpPost]
        [AllowAnonymous]
        [Route("UserLogin")]
        [ValidateAntiForgeryToken(LoggerName = LoggerName)]
        public IHttpActionResult UserLogin(LoginUserBindingModel _LoginUserBindingModel)
        {
            ApplicationLogger.WriteTrace("Start UserLogin", LoggerName);
            string JWTToken = string.Empty;
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);
                ApplicationLogger.WriteError(messages, LoggerName);
                return BadRequest(ModelState);
            }

            try
            {
                JWTToken = (new UserAccount()).GetJWTToken(_LoginUserBindingModel,
                                                        HttpContext.Current.Request.UserHostAddress,
                                                        HttpContext.Current.Request.UserAgent);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End UserLogin", LoggerName);
            return Ok(JWTToken);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UserLogout")]
        [ValidateAntiForgeryToken(LoggerName = LoggerName)]
        [CustomizedAuthorization(LoggerName = LoggerName)]
        public IHttpActionResult UserLogout()
        {
            ApplicationLogger.WriteTrace("Start UserLogout", LoggerName);
            try
            {
                string EncodedJWTToken = Request.Headers.Authorization.Parameter;
                (new UserAccount()).RemoveActiveUser(EncodedJWTToken);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End UserLogout", LoggerName);
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CreateUser")]
        [ValidateAntiForgeryToken(LoggerName = LoggerName)]
        public IHttpActionResult CreateUser(CreateUserBindingModel _CreateUserBindingModel)
        {
            ApplicationLogger.WriteTrace("Start CreateUser", LoggerName);
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);
                ApplicationLogger.WriteError(messages, LoggerName);
                return BadRequest(ModelState);
            }

            try
            {
                (new UserAccount()).RegisterUser(_CreateUserBindingModel);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End CreateUser", LoggerName);
            return Ok();
        }
    }
}