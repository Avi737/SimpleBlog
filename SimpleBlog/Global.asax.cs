using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleBlog.App_Start;

namespace SimpleBlog
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // The database will be invoke at the begining of our application
            Database.Configure();
        }

        protected void Application_BeginRequest()
        {
            Database.OpenSession(); // open up a session at the beginning of every request.
        }

        protected void Application_EndRequest()
        {
            Database.CloseSession(); // close the session at the end of the request.
        }

    }
}
