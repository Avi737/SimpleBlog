using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SimpleBlog.Controllers;

namespace SimpleBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            var namespaces = new[] {typeof(PostsController).Namespace};

            // Login Route
            routes.MapRoute("Login", "login", new {controller = "Auth", action = "Login"},namespaces);

        
            //Home Route
            routes.MapRoute("Home", "", new {controller = "Posts", action = "Index"}, namespaces);

        }
    }
}
