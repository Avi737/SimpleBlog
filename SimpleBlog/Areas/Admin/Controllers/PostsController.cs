using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;

namespace SimpleBlog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("posts")]
    public class PostsController:Controller
    {
        private const int PostsPerPage = 5;
        public ActionResult Index(int page = 1)
        {
            // count of how many posts we have in the db , post that we on the current page.
            // we need a get parameter to determine what page we're on, and default it to one if no parameters been specified.
            var totalPostCount = Database.Session.Query<Post>().Count();
            // getting the current page but first we need to know how many items per page
            var currentPostPage = Database.Session.Query<Post>()
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PostsPerPage) //if page is 1 we skip 0 items etc,..
                .Take(PostsPerPage) // take 5(give us a slice of the pages based on what page we're on)
                .ToList();

            return View(new PostsIndex
            {
                Posts =  new PageData<Post>(currentPostPage,totalPostCount,page,PostsPerPage)
            });
        }
        
    }
}