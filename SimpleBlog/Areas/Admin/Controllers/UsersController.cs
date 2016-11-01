using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Converters;
using NHibernate.Id;
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
                Roles = Database.Session.Query<Role>().Select(role => new RoleCheckbox
                {
                  Id =role.Id,
                  IsChecked = false,
                  Name = role.Name
                }).ToList()
            });
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult New(UsersNew form)
        {

            var user = new User();
            SyncRoles(form.Roles,user.Roles); // sync the roles and adding them to the user.



            // Validation for existing user
            if (Database.Session.Query<User>().Any(u => u.Username == form.Username))
                ModelState.AddModelError("Username","Username must be unique");

            // user is valid
            if (!ModelState.IsValid)
                return View(form); // if the validation faild we're goint to exist out of this method and not saving the data.

            //Creating User Entity and save it into the database, if we passed validation and the username is unique
            user.Email = form.Email;
            user.Username = form.Username;
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
                Email = user.Email,
                Roles = Database.Session.Query<Role>().Select(role => new RoleCheckbox
                {
                    Id = role.Id,
                    IsChecked = user.Roles.Contains(role), // checking if the user already have that role.
                    Name = role.Name
                }).ToList()
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

            SyncRoles(form.Roles,user.Roles);

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


        // Logic that will sync the two collections, so they're identical
        // the checkboxes represent what the view is seen, and the roles is our model.
        private void SyncRoles(IList<RoleCheckbox> checkboxes, IList<Role> roles)
        {
            var selectedRoles = new List<Role>();

            // Querying all the roles in the database.
            foreach (var role in Database.Session.Query<Role>())
            {
                var checkbox = checkboxes.Single(c => c.Id == role.Id);
                checkbox.Name = role.Name; // it will update the names of our checkboxes

                if (checkbox.IsChecked) // add it to our selectedRoles
                    selectedRoles.Add(role);
            }

            // Here we're going to check if we need to add role to the user or remove the role from the user.
            // for each role that in the selected roles but not in our user roles table.
            foreach (var toAdd in selectedRoles.Where(t => !roles.Contains(t)))
                roles.Add(toAdd);

            // remove a role by looping through all the roles of our current user if they don't exist in the selected roles then we remove them.
            // ToList means that we're doing the calculation then adding it into the list and removing the role from that calculated list.
            foreach (var toRemove in roles.Where(t => !selectedRoles.Contains(t)).ToList())
                roles.Remove(toRemove);
        }

    }
}