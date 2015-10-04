using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.IdentityModel.Tokens;
using SG50.Model.Models.Entities;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace SG50.Common
{   
    public class CustomizedAuthorizationAttribute : AuthorizationFilterAttribute
    {   
        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            //if (!principal.Identity.IsAuthenticated)
            //{
            //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            //    return Task.FromResult<object>(null);
            //}

            //if (!(principal.HasClaim(x => x.Type == ClaimType && x.Value == ClaimValue)))
            //{
            //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            //    return Task.FromResult<object>(null);
            //}

            //actionContext.Request.Headers.

            if (actionContext.Request.Headers.Authorization.Parameter == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            var _JwtSecurityToken = new JwtSecurityToken(actionContext.Request.Headers.Authorization.Parameter);
            string AudienceId = _JwtSecurityToken.Audiences.First();
            string SecurityKey = string.Empty;

            using(SG50DBEntities _SG50DBEntities = new SG50DBEntities())
            {
                tbl_AppActiveUser _tbl_AppActiveUser =  _SG50DBEntities.tbl_AppActiveUser.Where(x => x.AppUserId.Equals(AudienceId, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (_tbl_AppActiveUser == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return Task.FromResult<object>(null);
                }
                SecurityKey = _tbl_AppActiveUser.JwtHMACKey;
            }

            var _JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var _JwtSecurityTokenHandler_JWTToken = _JwtSecurityTokenHandler.ReadToken(actionContext.Request.Headers.Authorization.Parameter);
           
            SecurityToken _SecurityToken = null;
            _JwtSecurityTokenHandler.ValidateToken(
                _JwtSecurityTokenHandler_JWTToken,
                new TokenValidationParameters(){
                 IssuerSigningKey = new InMemorySymmetricSecurityKey(TextEncodings.Base64Url.Decode(SecurityKey))
                }, _SecurityToken);
            

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);
        }
    }
}
