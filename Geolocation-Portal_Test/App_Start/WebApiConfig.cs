using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using Geolocation_Portal_Test.Models;

namespace Geolocation_Portal_Test
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web-API-Konfiguration und -Dienste
            config.Formatters.JsonFormatter.SupportedMediaTypes
    .Add(new MediaTypeHeaderValue("text/html"));

            // Web-API-Routen
            config.MapHttpAttributeRoutes();

            /*config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<record>("records");
            builder.EntitySet<category>("categories");
            builder.EntitySet<file>("files");
            builder.EntitySet<location>("locations");
            builder.EntitySet<publisher>("publishers");
            builder.EntitySet<licence>("licences");
            builder.EntitySet<comment>("comments");
            builder.EntitySet<user>("users");
            builder.EntitySet<role>("role");
            builder.EntitySet<searchtag>("searchtags");
            config.Routes.MapODataServiceRoute("odata", "api", builder.GetEdmModel());
        }
    }
}
