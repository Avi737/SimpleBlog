using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleBlog.Models;

namespace SimpleBlog.Areas.Admin.ViewModels
{
    public class UsersIndex
    {
        public IEnumerable<User> Users { get; set; }
        //Collection of users that we can represent on our admin form.(List of the users)

    }
}