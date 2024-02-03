using System;
using MyBlog.BL.Auth;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Interfaces
{
    public interface IUserSecurityDAL
    {
        Task<int> AddUserSecurity(UserSecurityModel model);

        Task<UserSecurityModel> GetUserSecurity(int userid);
    }
}

