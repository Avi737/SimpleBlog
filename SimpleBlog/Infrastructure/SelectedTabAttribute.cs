using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlog.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SelectedTabAttribute:ActionFilterAttribute
    {
        // Which tab is been selected
        private readonly string _selectedTab;
        public SelectedTabAttribute(string selectedTab)
        {
            _selectedTab = selectedTab;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

            filterContext.Controller.ViewBag.SelectedTab = _selectedTab;
        }

    }
}