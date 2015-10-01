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

namespace SG50.TokenService.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountController : BaseApiController
    {
        const string RouteName_ConfirmEmail = "ConfirmEmailRoute";
        const string RouteName_GetUserById = "GetUserById";
        string Required_ID_Code = "User Id and Code are required";
        string Email_Subject = "Confirm your account";
        string Email_Body = "Please confirm your account by clicking <a href=\"{0}\">here</a>";
        int SaltKeyLength = 16;

        [HttpPost]
        [AllowAnonymous]  
        [Route("UserLogin", Name = "UserLoginRoute")]        
        [ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> UserLogin(LoginUserBindingModel loginUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IHttpActionResult _IHttpActionResult;
            HttpResponseMessage _HttpResponseMessage = new HttpResponseMessage();
            Cookie _Cookie = new Cookie();
            _Cookie.Name = "CookieName";
            _Cookie.Value = "CookieValue";
            _Cookie.HttpOnly = true;
            _Cookie.Secure = true;
            _HttpResponseMessage.Headers.SetCookie(_Cookie);
            _IHttpActionResult = ResponseMessage(_HttpResponseMessage);

            return Ok(_IHttpActionResult);            
        }

        [HttpPost]        
        [AllowAnonymous]        
        [Route("CreateUser", Name = "CreateUserRoute")]        
        [ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var SaltKey_ByteArray = Security.GetSaltKey(SaltKeyLength);
            var user = new ApplicationUser()
            {                
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,                
                Email = createUserModel.Email,
                UserName = createUserModel.UserName,
                JoinDate = createUserModel.JoinDate,
                SaltKey = Convert.ToBase64String(SaltKey_ByteArray),
                IsActive = true,
                CreatedDate = DateTime.Now,
                CreatedBy = createUserModel.UserName,
                UpdateDate = null,
                UpdateBy = null
            };

            //string Hashed_Password = Security.HashCode(Converter.GetBytes(createUserModel.Password), SaltKey_ByteArray);
            //IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, Hashed_Password);
            //if (!addUserResult.Succeeded)
            //{
            //    return GetErrorResult(addUserResult);
            //}
            
            /*
            //string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            string code = "883e31d0-beca-4f08-9e6e-78355be2e829";

            try
            {
                var callbackUrl = new Uri(Url.Link(RouteName_ConfirmEmail, new { userId = user.Id, code = code }));

                //await this.AppUserManager.SendEmailAsync(user.Id,
                //                                        Email_Subject,
                //                                        string.Format(Email_Body, callbackUrl));

                await this.AppUserManager.SendEmailAsync("090f8e1b-26b8-4432-a89f-2121f7584173",
                                                        Email_Subject,
                                                        string.Format(Email_Body, callbackUrl));
            }
            catch (Exception ex) {
                throw ex;
            }
            

            //Uri locationHeader = new Uri(Url.Link(RouteName_GetUserById, new { id = user.Id }));
            Uri locationHeader = new Uri(Url.Link(RouteName_GetUserById, new { id = "090f8e1b-26b8-4432-a89f-2121f7584173" }));
            */

            //return Created(locationHeader, TheModelFactory.Create(user));
            return Ok();

        }

        [HttpGet]
        [AllowAnonymous]        
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError(string.Empty, Required_ID_Code);
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }
    }
}
