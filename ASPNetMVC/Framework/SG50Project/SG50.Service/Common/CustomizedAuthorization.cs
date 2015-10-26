using SG50.Base.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using SG50.Model.BusinessLogic;

namespace SG50.Service.Common
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

                bool IsValidToken = (new AuthTokenChecker()).IsTokenAuthorized(EncodedJwtToken,
                                        ((HttpContextWrapper)actionContext.Request.Properties[MS_HttpContext]).Request.UserHostName,
                                        ((HttpContextWrapper)actionContext.Request.Properties[MS_HttpContext]).Request.UserAgent);

                if (!IsValidToken)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    return Task.FromResult<object>(null);
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