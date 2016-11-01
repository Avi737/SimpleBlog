using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;

namespace SimpleBlog.ViewModels
{
    public class PostsIndex
    {
        public PageData<Post> Posts { get; set; }

    }

    public class PostsShow
    {
        public Post Post { get; set; }
    }

    public class PostsTag
    {
        public Tag Tag { get; set; }
        public PageData<Post> Posts { get; set; }
    }
}