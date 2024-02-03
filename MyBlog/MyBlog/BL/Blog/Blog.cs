using System;
using MyBlog.DAL.Models;
using MyBlog.DAL.Interfaces;
using MyBlog.BL.Auth;
using Microsoft.Extensions.DependencyInjection;


namespace MyBlog.BL.Blog
{
    public class Blog : IBlog
    {
        private readonly IBlogDAL blogDAL;

        public Blog(IBlogDAL blogDAL)
        {
            this.blogDAL = blogDAL;
        }

        public async Task<int> Add(BlogModel model)
        {
            model.Created = DateTime.Now;
            model.Status = 0;
            return await blogDAL.Add(model);
        }

        public async Task<IEnumerable<BlogModel>> List(string status)
        {
            return await blogDAL.List(status);
        }
    }
}

