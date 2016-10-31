using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlog.Infrastructure
{
    public class TransactionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Database.Session.BeginTransaction();
        }
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Means there was no exception throwing by the action
            if (filterContext.Exception == null)
                Database.Session.Transaction.Commit(); // if there's no error commit the transaction
            else
                Database.Session.Transaction.Rollback(); // rollback if there was an error occured during a transaction.
        }

       
    }
}