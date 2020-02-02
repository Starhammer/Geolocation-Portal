using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Geolocation_Portal_Test
{
    public class RouteConfig
    {
        public Controllers.BenutzerController BenutzerController
        {
            get => default;
            set
            {
            }
        }

        public Controllers.HomeController HomeController
        {
            get => default;
            set
            {
            }
        }

        public Controllers.OpenDataController OpenDataController
        {
            get => default;
            set
            {
            }
        }

        public Controllers.FAQController FAQController
        {
            get => default;
            set
            {
            }
        }

        public Controllers.GeoDataController GeoDataController
        {
            get => default;
            set
            {
            }
        }

        public Controllers.DiagramDataController DiagramDataController
        {
            get => default;
            set
            {
            }
        }

        public Controllers.DataController DataController
        {
            get => default;
            set
            {
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "OpenData",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "OpenData", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "GeoData",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "GeoData", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DiagramData",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "DiagramData", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "FAQ",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "FAQ", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
