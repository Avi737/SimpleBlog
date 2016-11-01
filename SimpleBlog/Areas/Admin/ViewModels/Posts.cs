using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;

namespace SimpleBlog.Areas.Admin.ViewModels
{
    public class PostsIndex
    {
        public PageData<Post> Posts { get; set; }

        public class PostsForm
        {

            // those are going to determin if it's a new form or edit form.
            public bool isNew { get; set; }
            public int? PostId { get; set; } 

            [Required,MaxLength(128)]
            public string Title { get; set; }
            [Required, MaxLength(128)]

            public string Slug { get; set; }
            [Required, DataType(DataType.MultilineText)]

            public string Content  { get; set; }
        }

    }
}