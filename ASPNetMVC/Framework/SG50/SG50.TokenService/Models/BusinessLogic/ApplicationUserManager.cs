﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SG50.TokenService.Models.Entities;
using SG50.TokenService.Models.POCO;
using SG50.TokenService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SG50.TokenService.Models.BusinessLogic
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext));
            
            appUserManager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                //appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                //{
                //    //Code for email confirmation and reset password life time
                //    TokenLifespan = TimeSpan.FromHours(6)
                //};
                
                /// create db, but not essential.
                // var AppUser = appUserManager.Users.FirstOrDefault();

                appUserManager.UserTokenProvider = new CustomizedEmailPasswordResetTokenProvider<ApplicationUser>();
            }

            return appUserManager;
        }
    }
}