using SG50.Base.Logging;
using SG50.Model.ViewModel;
using SG50.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SG50.Service.Controllers
{
    [RoutePrefix("api/country")]
    public class CountryController : BasedController
    {
        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken(LoggerName = LoggerName)]
        [CustomizedAuthorization(LoggerName = LoggerName)]
        public IHttpActionResult Create(CountryBindingModel _LoginUserBindingModel)
        {
            ApplicationLogger.WriteTrace("Start CountryController Create", LoggerName);
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
                //JWTToken = (new UserAccount()).GetJWTToken(_LoginUserBindingModel,
                //                                        HttpContext.Current.Request.UserHostAddress,
                //                                        HttpContext.Current.Request.UserAgent);
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End CountryController Create", LoggerName);
            return Ok(JWTToken);
        }
    }
}
