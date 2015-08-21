using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FingerPrintingAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Add(new BinaryMediaTypeFormatter());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{file}",
                defaults: new { file = RouteParameter.Optional }    
            );
        }
    }
}
