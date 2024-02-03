using System;
using System.Linq;
using MyBlog.DAL.Models;
using MyBlog.DAL.Interfaces;

namespace MyBlog.BL.Auth
{
    public class Session: ISession
    {
        private readonly ISessionDAL sessionDAL;
        private readonly IHttpContextAccessor httpContextAccessor;

        public Session(ISessionDAL sessionDAL, IHttpContextAccessor httpContextAccessor)
        {
            this.sessionDAL = sessionDAL;
            this.httpContextAccessor = httpContextAccessor;
        }

        private void CreateSessionCookie(Guid sessionid)
        {
            CookieOptions options = new CookieOptions();
            options.Path = "/";
            options.HttpOnly = true;
            options.Secure = true;
            httpContextAccessor?.HttpContext?.Response.Cookies.Delete(General.Constants.SessionCookieName);
            httpContextAccessor?.HttpContext?.Response.Cookies.Append(General.Constants.SessionCookieName, sessionid.ToString(), options);
        }

        private async Task<SessionModel> CreateSession()
        {
            var data = new SessionModel()
            {
                SessionId = Guid.NewGuid(),
                Created = DateTime.Now,
                LastAccessed = DateTime.Now
            };
            await sessionDAL.CreateSession(data);
            return data;
        }

        public async Task<SessionModel> GetSession()
        {
            Guid sessionId;
            var cookie = httpContextAccessor?.HttpContext?.Request?.Cookies.FirstOrDefault(m => m.Key == General.Constants.SessionCookieName);
            if (cookie != null && cookie.Value.Value != null)
                sessionId = Guid.Parse(cookie.Value.Value);
            else
            {
                sessionId = Guid.NewGuid();
                CreateSessionCookie(sessionId);
                return await this.CreateSession();
            }

            var data = await this.sessionDAL.GetSession(sessionId);
            if (data == null)
            {
                data = await this.CreateSession();
                CreateSessionCookie(data.SessionId);
            }
            return data;
        }

        public async Task<int> SetUserId(int userId)
        {
            var data = await this.GetSession();
            data.UserId = userId;
            data.SessionId = Guid.NewGuid();
            CreateSessionCookie(data.SessionId);
            return await sessionDAL.CreateSession(data);
        }

        public async Task<int?> GetUserId()
        {
            var data = await this.GetSession();
            return data.UserId;
        }

        public async Task<bool> IsLoggedIn()
        {
            var data = await this.GetSession();
            return data.UserId != null;
        }
    }
}

