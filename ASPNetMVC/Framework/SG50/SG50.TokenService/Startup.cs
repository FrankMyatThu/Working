using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using SG50.TokenService.Models.BusinessLogic;
using SG50.TokenService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;

// Tell OWIN to start with this
[assembly: OwinStartup(typeof(SG50.TokenService.Startup))]
namespace SG50.TokenService
{
    public class Startup
    {
        NameValueCollection AppSettings = ConfigurationManager.AppSettings;
        string CorsOrigins = "cors:Origins";
        string CorsHeaders = "cors:Headers";
        string CorsMethods = "cors:Methods";
        string UriTokenPath = "uri:tokenpath";
        string UriLoginPath = "uri:loginpath";
        string UrlTokenIssuer = "url:tokenIssuer";         

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            
            ConfigureOAuthTokenGeneration(app);            
            ConfigureWebApi(httpConfig);

            var corsPolicy = new EnableCorsAttribute(
                            origins: AppSettings[CorsOrigins],
                            headers: AppSettings[CorsHeaders],
                            methods: AppSettings[CorsMethods]);

            // Enable CORS for ASP.NET Identity
            app.UseCors(new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = request =>
                        request.Path.Value == AppSettings[UriTokenPath] ?
                        corsPolicy.GetCorsPolicyAsync(null, CancellationToken.None) :
                        Task.FromResult<CorsPolicy>(null)
                }
            });

            // Enable CORS for Web API
            httpConfig.EnableCors(corsPolicy);

            app.UseWebApi(httpConfig);
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(AppSettings[UriLoginPath]),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat(AppSettings[UrlTokenIssuer])
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        

        private void ConfigureWebApi(HttpConfiguration config)
        {
            //WebApiConfig.Register(config);
            config.MapHttpAttributeRoutes();
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}