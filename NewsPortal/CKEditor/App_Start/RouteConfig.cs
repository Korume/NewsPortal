﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NewsPortal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "MainNewsPage",
                url: "{controller}/{newsItemId}",
                defaults: new { controller = "News", action = "MainNews" },
                constraints: new { newsItemId = @"\d+" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{*other}",
                defaults: new { controller = "Home", action = "Index" }
                );
        }
    }
}
