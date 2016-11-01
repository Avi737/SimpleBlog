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


        public ActionResult New()
        {
            
            // combining the new and the edit into the same view model.
            return View("Form", new PostsIndex.PostsForm
            {
                isNew = true
            });
        }

        public ActionResult Edit(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
                return HttpNotFound();

            return View("Form", new PostsIndex.PostsForm
            {
                isNew =  false,
                PostId = id,
                Content = post.Content,
                Slug = post.Slug,
                Title = post.Title

            });


        }



        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Form(PostsIndex.PostsForm form)
        {
            form.isNew = form.PostId == null;
            if(!ModelState.IsValid)
                return View(form); // if there's an error we'll dispaly what is the error on the screen.


            // New post
            Post post;
            if (form.isNew)
            {
                post = new Post
                {
                    CreatedAt = DateTime.Now,
                    User = Auth.User
                };
            }
            else
            {
                // Update existing post.
                // if we're not publishing a new form, we need to get a form from the database.
                post = Database.Session.Load<Post>(form.PostId);
                if(post == null)
                    return HttpNotFound();

                post.UpdatedAt = DateTime.Now;
            }

            post.Title = form.Title;
            post.Slug = form.Slug;
            post.Content = form.Content;

            // Save a new copy of our entity if it's already exist or updated is it does.
            Database.Session.SaveOrUpdate(post);

            return RedirectToAction("index");

        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Trash(int id)
        {

            var post = Database.Session.Load<Post>(id);
            if(post == null)
                return HttpNotFound();

            post.DeletedAt = DateTime.Now;
            Database.Session.Update(post);
            return RedirectToAction("index");

        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
                return HttpNotFound();

            post.DeletedAt = DateTime.Now;
            Database.Session.Delete(post);
            return RedirectToAction("index");
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Restore(int id)
        {
            var post = Database.Session.Load<Post>(id);
            if (post == null)
                return HttpNotFound();

            post.DeletedAt = null;
            Database.Session.Update(post);
            return RedirectToAction("index");
        }
    }
}