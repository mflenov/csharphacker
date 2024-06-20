using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
    public class CurrentUser: ICurrentUser
    {
        private readonly IEncrypt encrypt;

        // очень плохо держать тут IHttpContextAccessor, его не должно быть в BL уровне
        // но для простоты кода в книге я оставлю его здесь, а на бусти https://boosty.to/mflenov
        // у меня есть видео создания электронного магазина и сайта на  .NET и там я сделал 
        // рефакторинг для и код там лучше. Здесь в коде есть файл WebCookie.cs, который нужно использовать 
        // вместо прямого доступа к httpContextAccessor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IAuthenticationDAL authenticationDAL;
        private readonly IAuthentication authentication;
        private readonly ISession session;
        private readonly IUserTokenDAL userTokenDAL;
        private readonly IWebCookie webCookie;

        public CurrentUser(IHttpContextAccessor httpContextAccessor,
            IAuthenticationDAL authenticationDAL,
            IAuthentication authentication,
            ISession session,
            IUserTokenDAL userTokenDAL,
            IWebCookie webCookie,
            IEncrypt encrypt)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.authenticationDAL = authenticationDAL;
            this.authentication = authentication;
            this.encrypt = encrypt;
            this.session = session;
            this.userTokenDAL = userTokenDAL;
            this.webCookie = webCookie;
        }

        private UserModel? userData = null;
        public async Task<UserModel> GetUserData()
        {
            if (userData == null)
            {
                bool isLoggedIn = await IsLoggedIn();
                if (isLoggedIn)
                {
                    userData = await authenticationDAL.GetUser(httpContextAccessor?.HttpContext?.Session.GetInt32("userid") ?? 0);
                    //userData = await authenticationDAL.GetUser((int)session.GetUserId().GetAwaiter().GetResult());
                }
                else
                    userData = new UserModel();                    
            }
            return userData!;
        }

		public async Task<int?> GetUserIdByToken()
		{
			string? tokenCookie = webCookie.Get(General.Constants.RememberMeCookieName);
			if (tokenCookie == null)
				return null;
			Guid? tokenGuid = General.Helpers.StringToGuidDef(tokenCookie ?? "");
			if (tokenGuid == null)
				return null;

            int? userid = await userTokenDAL.Get((Guid)tokenGuid);
			return userid;
        }


        public async Task<bool> IsLoggedIn()
        {
            bool loggedIn = this.httpContextAccessor?.HttpContext?.Session.GetInt32("userid") != null;

            // не используйте GetAwaiter().GetResult(), я использую это здесь только потому
            // что не хочу рефакторить код и оставить его так, как я его приводил во второй главе
            //bool loggedIn = session.IsLoggedIn().GetAwaiter().GetResult();
            if (!loggedIn) // Запомни меня
            {
                // Это вариант с уникальными токенами
				int? userid = await GetUserIdByToken();
				if (userid != null)
				{
					await session.SetUserId((int)userid);
					loggedIn = true;
                }

                /* 
                // ЭТО ВАРИАНТ С шифрованием UserId. Он работает, но небезопасный
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
                */
            }
            return loggedIn;
        }
    }
}

