using System;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Interfaces
{
    public interface ISessionDAL
    {
        Task<SessionModel?> GetSession(Guid sessionId);

        Task<int> UpdateSession(SessionModel model);

        Task<int> CreateSession(SessionModel model);

        Task Extend(Guid dbSessionID);

        Task Delete(Guid sessionId);
    }
}

