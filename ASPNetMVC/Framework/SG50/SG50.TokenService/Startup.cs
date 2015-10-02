using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using SG50.Base.Util;
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
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();                        
            ConfigureWebApi(httpConfig);

            var corsPolicy = new EnableCorsAttribute(                                    
                                    origins: AppConfiger.CorsOrigins,
                                    headers: AppConfiger.CorsHeaders,
                                    methods: AppConfiger.CorsMethods);

            //// Enable CORS for Web API
            httpConfig.EnableCors(corsPolicy);            
            app.UseWebApi(httpConfig);
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
            WebApiConfig.Register(config);
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}