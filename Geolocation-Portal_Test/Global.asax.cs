using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Geolocation_Portal_Test
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public BundleConfig BundleConfig
        {
            get => default;
            set
            {
            }
        }

        public FilterConfig FilterConfig
        {
            get => default;
            set
            {
            }
        }

        public Models.Entities Entities
        {
            get => default;
            set
            {
            }
        }

        public RouteConfig RouteConfig
        {
            get => default;
            set
            {
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
