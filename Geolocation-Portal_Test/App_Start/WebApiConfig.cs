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
            builder.EntitySet<record>("record");
            builder.EntitySet<category>("category");
            builder.EntitySet<file>("file");
            builder.EntitySet<location>("location");
            builder.EntitySet<publisher>("publisher");
            builder.EntitySet<licence>("licence");
            builder.EntitySet<comment>("comment");
            builder.EntitySet<user>("user");
            builder.EntitySet<role>("role");
            config.Routes.MapODataServiceRoute("odata", "api", builder.GetEdmModel());
        }
    }
}
