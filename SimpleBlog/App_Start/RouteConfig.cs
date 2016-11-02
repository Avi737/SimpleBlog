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

            var namespaces = new[] { typeof(PostsController).Namespace };
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute("TagForRealThisTime", "tag/{idAndSlug}", new { Controller = "Posts", action = "Tag" }, namespaces);
            routes.MapRoute("Tag", "tag/{id}-{slug}", new { Controller = "Posts", action = "Tag" }, namespaces);

            routes.MapRoute("PostForRealThisTime","post/{idAndSlug}",new {Controller="Posts", action="Show"},namespaces);
            routes.MapRoute("Post", "post/{id}-{slug}", new {Controller = "Posts", action ="Show"}, namespaces);
          
            // Login Route
            routes.MapRoute("Login", "login", new {controller = "Auth", action = "Login"},namespaces);

            // Logout Route
            routes.MapRoute("Logout", "logout", new { controller = "Auth", action = "Logout" }, namespaces);

            //Home Route
            routes.MapRoute("Home", "", new {controller = "Posts", action = "Index"}, namespaces);

            routes.MapRoute("Sidebar", "", new { Controller = "Layout", action = "Sidebar" }, namespaces);



        }
    }
}
