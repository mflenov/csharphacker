using System;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Interfaces
{
    public interface IBlogDAL
    {
        Task<int> Add(BlogModel model);
        Task<int> Update(BlogModel model);
        Task<IEnumerable<BlogModel>> List(string status);
    }
}

