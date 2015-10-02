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

namespace SG50.TokenService.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {    
        string Key_ModelStateInvalidError = "Key_ModelStateInvalidError";

        [HttpPost]
        [AllowAnonymous]  
        [Route("UserLogin")]        
        [ValidateAntiForgeryToken]
        public IHttpActionResult UserLogin(LoginUserBindingModel _LoginUserBindingModel)
        {
            string JWTToken = string.Empty;
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);
                return BadRequest(ModelState);
            }

            try
            {
                JWTToken = (new UserAccountBusinessLogic()).GetJWTToken(_LoginUserBindingModel);
            }
            catch (Exception ex) {
                return InternalServerError(ex);
            }

            return Ok(JWTToken);            
        }

        [HttpPost]        
        [AllowAnonymous]        
        [Route("CreateUser")]        
        [ValidateAntiForgeryToken]
        public IHttpActionResult CreateUser(CreateUserBindingModel _CreateUserBindingModel)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage));
                ModelState.AddModelError(Key_ModelStateInvalidError, messages);
                return BadRequest(ModelState);
            }

            try
            {
                (new UserAccountBusinessLogic()).RegisterUser(_CreateUserBindingModel);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }
    }
}
