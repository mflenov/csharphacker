using System;
using MyBlog.DAL.Models;

namespace MyBlog.BL.Blog
{
    public interface IBlog
    {
        Task<int> Add(BlogModel model);

        Task<IEnumerable<BlogModel>> List(string status);
    }
}

