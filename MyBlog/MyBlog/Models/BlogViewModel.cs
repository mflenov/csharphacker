using System;
using MyBlog.DAL.Models;

namespace MyBlog.Models
{
    public class BlogViewModel
    {
        public string? Title { get; set; }

        public string? Content { get; set; }

        public BlogModel ToBlogModel()
        {
            return new BlogModel()
            {
                Title = this.Title,
                Content = this.Content
            };
        }
    }
}

