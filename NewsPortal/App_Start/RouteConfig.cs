using System;
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

            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: "MainNewsPage",
                url: "{controller}/{title}-{newsItemId}",
                defaults: new { controller = "News", action = "MainNews" },
                constraints: new { newsItemId = @"\d+" }
                );

            routes.MapRoute(
                name: "NewsAdding",
                url: "news-adding",
                defaults: new { controller = "News", action = "Add" }
                );

            routes.MapRoute(
                name: "NewsEdit",
                url: "news-editings/{newsItemId}",
                defaults: new { controller = "News", action = "Edit" },
                constraints: new { newsItemId = @"\d+" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}
