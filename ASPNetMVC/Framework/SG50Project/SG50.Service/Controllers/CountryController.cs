using SG50.Base.Logging;
using SG50.Model.BusinessLogic;
using SG50.Model.POCO;
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
        [Route("GetCountry")]
        [ValidateAntiForgeryToken(LoggerName = LoggerName)]
        [CustomizedAuthorization(LoggerName = LoggerName)]
        public IHttpActionResult GetCountry(Country_Criteria_Model _Country_Criteria_Model)
        {
            ApplicationLogger.WriteTrace("Start CountryController GetCountry", LoggerName);
            List<tbl_GridListing<CountryBindingModel>> List_tbl_Country = new List<tbl_GridListing<CountryBindingModel>>();            
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
                List_tbl_Country = (new Country()).GetCountryList(_Country_Criteria_Model);                
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End CountryController GetCountry", LoggerName);
            return Ok(List_tbl_Country);              
        }

        [HttpPost]
        [Route("Create")]
        [ValidateAntiForgeryToken(LoggerName = LoggerName)]
        [CustomizedAuthorization(LoggerName = LoggerName)]
        public IHttpActionResult Create(CountryBindingModel _CountryBindingModel)
        {
            ApplicationLogger.WriteTrace("Start CountryController Create", LoggerName);            
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
            return Ok();
        }

        [HttpPost]
        [Route("Delete")]
        [ValidateAntiForgeryToken(LoggerName = LoggerName)]
        [CustomizedAuthorization(LoggerName = LoggerName)]
        public IHttpActionResult Delete(List<Country_Criteria_Model> List_Country_Criteria_Model)
        {
            ApplicationLogger.WriteTrace("Start CountryController Delete", LoggerName);            
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
                (new Country()).Delete(List_Country_Criteria_Model);      
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                return InternalServerError(ex);
            }

            ApplicationLogger.WriteTrace("End CountryController Delete", LoggerName);
            return Ok();
        }
    }
}
