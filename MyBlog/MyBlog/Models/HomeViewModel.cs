using System;
using Microsoft.AspNetCore.RateLimiting;
using MyBlog.DAL.Models;

namespace MyBlog.Models
{
    public class HomeViewModel
    {
        public bool IsLoggedIn { get; set; }

        public IEnumerable<BlogModel>? BlogItems { get; set; }
    }
}

