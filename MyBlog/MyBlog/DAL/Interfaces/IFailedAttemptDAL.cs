using System;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Interfaces
{
    public interface IFailedAttemptDAL
    {
        Task<int> AddFailedAttempt(string email, int userid, string ip);

        Task<int> AddFailedAttempt(string email, string ip);

        Task<int> GetFailedAttemptByEmail(string email, int minutes);

        Task<int> GetFailedAttemptByIp(string ip, int minutes);
    }
}

