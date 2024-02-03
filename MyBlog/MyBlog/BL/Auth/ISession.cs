using System;
using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
    public interface ISession
    {
        Task<SessionModel> GetSession();

        Task<int> SetUserId(int userId);

        Task<int?> GetUserId();

        Task<bool> IsLoggedIn();
    }
}

