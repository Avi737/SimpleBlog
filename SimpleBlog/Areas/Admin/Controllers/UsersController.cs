using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;
using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using static SimpleBlog.Areas.Admin.ViewModels.UsersIndex;

namespace SimpleBlog.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [SelectedTab("users")]
    public class UsersController:Controller
    {
        public ActionResult Index()
        {
            return View(new UsersIndex()
            {
                Users = Database.Session.Query<User>().ToList()
            });
        }

        public ActionResult New()
        {

            return View(new UsersNew
            {

            });
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult New(UsersNew form)
        {
            // Validation for existing user
            if (Database.Session.Query<User>().Any(u => u.Username == form.Username))
                ModelState.AddModelError("Username","Username must be unique");

            // user is valid
            if (!ModelState.IsValid)
                return View(form);

            //Creating User Entity and save it into the database, if we passed validation and the username is unique
            var user = new User
            {
                Email = form.Email,
                Username = form.Username
            };

            user.SetPassword(form.Password);

            //Save the user
            Database.Session.Save(user);
            return RedirectToAction("index");

        }


        public ActionResult Edit(int id)
        {
            var user = Database.Session.Load<User>(id); // load a user with the id of id
            if(user == null)
                return HttpNotFound(); // user wasn't found.

            // we want to pre populate the fields that we have in the database.
            return View(new UsersEdit
            {
                Username = user.Username,
                Email = user.Email
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UsersEdit form)
        {

            // the id comes from the URL and not from the form.
            // retrieve the user from the database.
            var user = Database.Session.Load<User>(id); // load a user with the id of id
            if (user == null)
                return HttpNotFound(); // user wasn't found.

            // What if the user decdied not to change his username.
            // if you find a username that is already in the db and it's not us.
            if(Database.Session.Query<User>().Any(u => u.Username == form.Username && u.Id != id))
                ModelState.AddModelError("Username","Username must be unique");

            // if the validation failed or added model errors we want to show the form to the user.
            if (!ModelState.IsValid)
                return View(form);


            // update our entity.
            user.Username = form.Username;
            user.Email = form.Email;
            Database.Session.Update(user);

            return RedirectToAction("index");

        }


        public ActionResult ResetPassword(int id)
        {
            var user = Database.Session.Load<User>(id); // load a user with the id of id
            if (user == null)
                return HttpNotFound(); // user wasn't found.

            // we want to pre populate the fields that we have in the database.
            return View(new UsersResetPassword
            {
                Username = user.Username
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int id, UsersResetPassword form)
        {
            // the id comes from the URL and not from the form.
            // retrieve the user from the database.
            var user = Database.Session.Load<User>(id); // load a user with the id of id
            if (user == null)
                return HttpNotFound(); // user wasn't found.

            // the controller is responsibile to populate the username not the form.
            form.Username = user.Username;
          

            // if the validation failed or added model errors we want to show the form to the user.
            if (!ModelState.IsValid)
                return View(form);


            // update our entity.
            user.SetPassword(form.Password);
            Database.Session.Update(user);

            return RedirectToAction("index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            // the id comes from the URL and not from the form.
            // retrieve the user from the database.
            var user = Database.Session.Load<User>(id); // load a user with the id of id
            if (user == null)
                return HttpNotFound(); // user wasn't found.

            Database.Session.Delete(user);
            return RedirectToAction("index");

        }

    }
}