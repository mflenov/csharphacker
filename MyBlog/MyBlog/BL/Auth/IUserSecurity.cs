using System;
using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
    public interface IUserSecurity
    {
        Task<int> CreateUserVerification(int userid, string email);

        Task<UserSecurityModel> GetUserSecurity(int userid);
    }
}

