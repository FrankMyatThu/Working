﻿using Microsoft.AspNet.Identity;
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

namespace SG50.TokenService.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountController : BaseApiController
    {
        const string RouteName_ConfirmEmailRoute = "ConfirmEmailRoute";
        const string RouteName_GetUserById = "GetUserById";
        string Required_ID_Code = "User Id and Code are required";
        string Email_Subject = "Confirm your account";
        string Email_Body = "Please confirm your account by clicking <a href=\"{0}\">here</a>";

        [AllowAnonymous]        
        [Route("create")]
        [CustomizedValidateAntiForgeryToken]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,                
                Email = createUserModel.Email,
                UserName = createUserModel.UserName,
                JoinDate = createUserModel.JoinDate,
            };


            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

            var callbackUrl = new Uri(Url.Link(RouteName_ConfirmEmailRoute, new { userId = user.Id, code = code }));

            await this.AppUserManager.SendEmailAsync(user.Id,
                                                    Email_Subject,
                                                    string.Format(Email_Body, callbackUrl));

            Uri locationHeader = new Uri(Url.Link(RouteName_GetUserById, new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = RouteName_ConfirmEmailRoute)]
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

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = RouteName_GetUserById)]
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
