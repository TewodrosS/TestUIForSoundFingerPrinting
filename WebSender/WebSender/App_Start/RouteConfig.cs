using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebSender
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "TrackData", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SendFile",
                url: "{controller}/{action}/{fileModel}",
                defaults: new { controller = "TrackData", action = "SendFile", fileModel = UrlParameter.Optional }
            );
        }
    }
}
