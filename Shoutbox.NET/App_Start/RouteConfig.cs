using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Shoutbox.NET
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Historie",
                url: "{controller}/Historie/{date}",
                defaults: new { controller = "Main", action = "Historie", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SSTStatus",
                url: "Main/Status/{service}",
                defaults: new { controller = "SSTStatus", action = "Get", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Tag",
                url: "Tag/{tag}",
                defaults: new { controller = "Main", action = "Tag", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "TodaysSerializedMessagesByDivision",
                url: "Message/GetTodayByDivisionSerialized/{division}",
                defaults: new { controller = "Message", Action = "GetTodayByDivisionSerialized", division = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
