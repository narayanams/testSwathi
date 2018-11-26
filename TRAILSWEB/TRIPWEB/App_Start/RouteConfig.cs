using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TRIPWEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MyReservation",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Reservation", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "TRIP",
               url: "TRIP",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
