using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
    public class CurrentUser: ICurrentUser
    {
        private readonly IEncrypt encrypt;

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAuthenticationDAL authenticationDAL;
        private readonly IAuthentication authentication;
        private readonly ISession session;

        public CurrentUser(IHttpContextAccessor httpContextAccessor,
            IAuthenticationDAL authenticationDAL,
            IAuthentication authentication,
            ISession session,
            IEncrypt encrypt)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.authenticationDAL = authenticationDAL;
            this.authentication = authentication;
            this.encrypt = encrypt;
            this.session = session;
        }

        private UserModel? userData = null;
        public async Task<UserModel> GetUserData()
        {
            if (userData == null)
            {
                if (IsLoggedIn())
                {
                    userData = await authenticationDAL.GetUser(httpContextAccessor?.HttpContext?.Session.GetInt32("userid") ?? 0);
                    //userData = await authenticationDAL.GetUser((int)session.GetUserId().GetAwaiter().GetResult());
                }
                else
                    userData = new UserModel();                    
            }
            return userData!;
        }

        public bool IsLoggedIn()
        {
            bool loggedIn = this.httpContextAccessor?.HttpContext?.Session.GetInt32("userid") != null;

            // не используйте GetAwaiter().GetResult(), я использую это здесь только потому
            // что не хочу рефакторить код и оставить его так, как я его приводил во второй главе
            //bool loggedIn = session.IsLoggedIn().GetAwaiter().GetResult();
            if (!loggedIn)
            {
                var cookie = httpContextAccessor?.HttpContext?.Request?.Cookies.FirstOrDefault(m => m.Key == General.Constants.RememberMeCookieName);
                if (cookie != null && cookie.Value.Value != null)
                {
                    try
                    {
                        var id = encrypt.Decrypt(cookie.Value.Value);
                        int? intid = General.Helpers.StringToIntDef(id, null);
                        if (intid != null)
                        {
                            httpContextAccessor?.HttpContext?.Session.SetInt32(
                                "userid", (int)intid
                            );
                            //session.SetUserId((int)intid);
                            return true;
                        }
                    }
                    catch(Exception)
                    {
                        return false;
                    }
                }
            }
            return loggedIn;
        }
    }
}

