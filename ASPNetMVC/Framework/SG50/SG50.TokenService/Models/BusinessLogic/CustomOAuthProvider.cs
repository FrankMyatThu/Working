using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.Owin;
using SG50.TokenService.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Cryptography;
using SG50.Base.Security;
using SG50.Base.Util;
using SG50.TokenService.Models.Entities;

namespace SG50.TokenService.Models.BusinessLogic
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        NameValueCollection AppSettings = ConfigurationManager.AppSettings;
        string TokenType = "JWT";
        string AccessControlAllowOrigin = "Access-Control-Allow-Origin";        
        string CorsOrigins = "cors:Origins";
        string InvalidGrant = "invalid_grant";
        string UserPasswordNotCorrect = "The user name or password is incorrect.";
        string UserNotConfirmEmail = "User did not confirm email.";
        string Claim_Column_AppUserID = "AppUserID";
        

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {   
            context.OwinContext.Response.Headers.Add(AccessControlAllowOrigin, new[] { AppSettings[CorsOrigins] });
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindByNameAsync(context.UserName);
            string Hashed_Password = Security.HashCode(Converter.GetBytes(user.Password), Convert.FromBase64String(user.SaltKey));
            user = await userManager.FindAsync(user.UserName, Hashed_Password);
            
            if (user == null)
            {
                context.SetError(InvalidGrant, UserPasswordNotCorrect);
                return;
            }

            if (!user.EmailConfirmed)
            {
                context.SetError(InvalidGrant, UserNotConfirmEmail);
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, TokenType);

            /// Application User ID
            oAuthIdentity.AddClaim(ClaimManager.GetClaim(Claim_Column_AppUserID, user.ID.ToString()));
            oAuthIdentity.AddClaims(ExtendedClaimsProvider.GetClaims(user));
            oAuthIdentity.AddClaims(RolesFromClaims.CreateRolesBasedOnClaims(oAuthIdentity));   
         
            var ticket = new AuthenticationTicket(oAuthIdentity, null);
            context.Validated(ticket);

        }
    }
}