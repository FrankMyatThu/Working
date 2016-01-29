using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using TestScript.Base.Util;

namespace TestScript.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            AccessToCors(config);
        }

        public static void AccessToCors(HttpConfiguration config)
        {
            var corsPolicy = new EnableCorsAttribute(
                                    origins: AppConfiger.CorsOrigins,
                                    headers: AppConfiger.CorsHeaders,
                                    methods: AppConfiger.CorsMethods);

            //// Enable CORS for Web API
            config.EnableCors(corsPolicy);
        }
    }
}
