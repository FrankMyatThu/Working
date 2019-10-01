using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ngNetCore.Base
{   
    public class CustomizedAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public string LoggerName { get; set; }
        const string Exception_ActionContext = "ActionContext is null";
        const string MS_HttpContext = "MS_HttpContext";

        public override Task OnAuthorizationAsync(HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                if (actionContext == null)
                {
                    throw new ArgumentNullException(Exception_ActionContext);
                }

                if (actionContext.Request.Headers.Authorization.Parameter == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return Task.FromResult<object>(null);
                }

                string Requester_IP = string.Empty;
                string Requester_UserAgent = string.Empty;
                string EncodedJwtToken = actionContext.Request.Headers.Authorization.Parameter;
                var _JwtSecurityToken = new JwtSecurityToken(EncodedJwtToken);
                string AudienceId = _JwtSecurityToken.Audiences.First();
                string SecurityKey = string.Empty;

                using(SG50DBEntities _SG50DBEntities = new SG50DBEntities())
                {
                    tbl_ActiveUser _tbl_ActiveUser = _SG50DBEntities.tbl_ActiveUser.Where(x => x.Id.Equals(new Guid(AudienceId))).FirstOrDefault();
                    if (_tbl_ActiveUser == null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return Task.FromResult<object>(null);
                    }
                    SecurityKey = _tbl_ActiveUser.JwtHMACKey;

                    var _JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                    var _JwtSecurityTokenHandler_JWTToken = _JwtSecurityTokenHandler.ReadToken(EncodedJwtToken);

                    /// (1) Validate Token
                    SecurityToken _SecurityToken = null;
                    _JwtSecurityTokenHandler.ValidateToken(
                        EncodedJwtToken,
                        new TokenValidationParameters()
                        {
                            IssuerSigningKey = new InMemorySymmetricSecurityKey(TextEncodings.Base64Url.Decode(SecurityKey)),
                            ValidAudience = _tbl_ActiveUser.Id.ToString(),                                                     
                            ValidIssuer = AppConfiger.UrlTokenIssuer,
                            ValidateLifetime = false,
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidateIssuerSigningKey = true
                        }, out _SecurityToken);

                    if (_SecurityToken == null)
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return Task.FromResult<object>(null);
                    }

                    /// (2) Validate if token is already expired.
                    if ((new LoginChecker()).IsUserIdle(_tbl_ActiveUser.LastRequestedTime))
                    {
                        /// Kick out user who is Idle or whose token is expired.
                        _SG50DBEntities.tbl_ActiveUser.Remove(_tbl_ActiveUser);
                        _SG50DBEntities.SaveChanges();
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return Task.FromResult<object>(null);
                    }

                    /// (3) Validate IP & User Agent                    
                    Requester_IP = ((HttpContextWrapper)actionContext.Request.Properties[MS_HttpContext]).Request.UserHostName;
                    Requester_UserAgent = ((HttpContextWrapper)actionContext.Request.Properties[MS_HttpContext]).Request.UserAgent;
                    if (!_tbl_ActiveUser.IP.Equals(Requester_IP, StringComparison.InvariantCultureIgnoreCase) ||
                        !_tbl_ActiveUser.UserAgent.Equals(Requester_UserAgent, StringComparison.InvariantCultureIgnoreCase))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        return Task.FromResult<object>(null);  
                    }

                    /// All Validation are passed.
                    /// So, Set current time to _tbl_ActiveUser.LastRequestedTime
                    /// in order to extend token expireation time.
                    _tbl_ActiveUser.LastRequestedTime = DateTime.Now;
                    _SG50DBEntities.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                BaseExceptionLogger.LogError(ex, LoggerName);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                return Task.FromResult<object>(null);
            }

            //User is Authorized, complete execution
            return Task.FromResult<object>(null);
        }
    }
}
