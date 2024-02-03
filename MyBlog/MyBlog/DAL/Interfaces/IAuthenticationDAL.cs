using System;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Interfaces
{
    public interface IAuthenticationDAL
    {
        Task<UserModel> GetUser(int id);

        Task<UserAuthModel> GetUser(string email);

        Task<UserAuthModel> GetUserByNormalizedEmail(string email);

        Task<int> CreateUser(UserModel user);

        Task<int> UpdatePassword(int userid, string password);
    }
}

