using Microsoft.AspNet.Identity;
using SG50.TokenService.Models.POCO;
using SG50.TokenService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SG50.Base.ForgeryProtector;
using SG50.Base.Security;
using SG50.Base.Util;
using System.Net.Http.Headers;
using SG50.TokenService.Models.BusinessLogic;
using SG50.Base.Logging;
using SG50.Common;

namespace SG50.TokenService.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {    
        string Key_ModelStateInvalidError = "Key_ModelStateInvalidError";
        const string LoggerName = "SG50_TokenService_Appender_Logger";

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
                JWTToken = (new UserAccountBusinessLogic()).GetJWTToken(_LoginUserBindingModel);
            }
            catch (Exception ex) {
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
                (new UserAccountBusinessLogic()).RemoveActiveUser(EncodedJWTToken);                
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
                (new UserAccountBusinessLogic()).RegisterUser(_CreateUserBindingModel);
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
