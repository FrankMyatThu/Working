using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(GeekQuiz.Service.Startup))]
namespace GeekQuiz.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {   
            ConfigureOAuth(app);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            #region Token Reader
            string audienceId = "GeekQuizClientApp";
            string symmetricKeyAsBase64 = "5YV7M1r981yoGhELyB84aC+KiYksxZf1OY3++C1CtRM=";
            var secret = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider("http://localhost:1622", secret)
                    },
                    Provider = new OAuthBearerAuthenticationProvider
                    {
                        OnValidateIdentity = context =>
                        {
                            context.Ticket.Identity.AddClaim(new System.Security.Claims.Claim("newCustomClaim", "newValue"));
                            return Task.FromResult<object>(null);
                        }
                    }
                }
            );
            #endregion
        }
    }
}