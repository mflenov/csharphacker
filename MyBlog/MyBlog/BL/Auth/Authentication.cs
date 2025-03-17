using System.ComponentModel.DataAnnotations;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
    public class Authentication: IAuthentication
    {
        private readonly IAuthenticationDAL authenticationDAL;
        private readonly IFailedAttemptDAL failedAttemptDAL;

        // очень плохо держать тут IHttpContextAccessor, его не должно быть в BL уровне
        // но для простоты кода в книге я оставлю его здесь, а на бусти https://boosty.to/mflenov
        // у меня есть видео создания электронного магазина и сайта на  .NET и там я сделал 
        // рефакторинг для и код там лучше. Здесь в коде есть файл WebCookie.cs, который нужно использовать 
        // вместо прямого доступа к httpContextAccessor
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEncrypt encrypt;
        private readonly IUserSecurity userSecurity;
        private readonly ISession session;
        private readonly IUserTokenDAL userTokenDAL;


        public Authentication(IAuthenticationDAL authenticationDAL,
            IFailedAttemptDAL failedAttemptDAL,
            IHttpContextAccessor httpContextAccessor, 
            IEncrypt encrypt,
            ISession session,
            IUserTokenDAL userTokenDAL,
            IUserSecurity userSecurity)
        {
            this.authenticationDAL = authenticationDAL;
            this.failedAttemptDAL = failedAttemptDAL;
            this.httpContextAccessor = httpContextAccessor;
            this.encrypt = encrypt;
            this.userSecurity = userSecurity;
            this.session = session;
            this.userTokenDAL = userTokenDAL;
        }

        public async Task<int> CreateUser(UserModel user)
        {
            user.Salt = Guid.NewGuid().ToString();
            user.Password = encrypt.HashPassword(user.Password, user.Salt);
            user.NormilizedEmail = MyBlog.BL.General.BlHelpers.NormilizeEmail(user.Email);

            int id = await authenticationDAL.CreateUser(user);
            await userSecurity.CreateUserVerification(id, user.Email);
            await this.Login(id, false);

            return id;
        }

        public async Task Login(int userid, bool rememberme)
        {
            httpContextAccessor?.HttpContext?.Session.SetInt32("userid", userid);

            if (rememberme)
            {
                // запоминаем себя по токену
                UserTokenModel tokenModel = new UserTokenModel() { 
                    UserId = userid,
                    UserTokenId = Guid.NewGuid(),
                    UserAgent = httpContextAccessor?.HttpContext?.Request.Headers["User-Agent"] ?? ""
                    };
                await userTokenDAL.Create(tokenModel);

                // это лучше вынести в отдельный класс
                CookieOptions options = new CookieOptions();
                options.Path = "/";
                options.HttpOnly = true;
                options.Secure = true;
                options.Expires = DateTimeOffset.UtcNow.AddDays(General.Constants.RememberMeDays);
                httpContextAccessor?.HttpContext?.Response.Cookies.Append(General.Constants.RememberMeCookieName, ((Guid)tokenModel.UserTokenId!).ToString(), options);

                // Вариант с шифрованием ID аккаунта
                //httpContextAccessor?.HttpContext?.Response.Cookies.Append(General.Constants.RememberMeCookieName, encrypt.Encrypt(id.ToString()), options);
            }
        }

        public async Task<ValidationResult?> ValidateEmail(string? email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                var user = await authenticationDAL.GetUserByNormalizedEmail(MyBlog.BL.General.BlHelpers.NormilizeEmail(email));
                if (user.UserId != null)
                    return new ValidationResult("Email уже существует");
            }
            return null;
        }

        public async Task<bool> Authenticate(String email, String password, String ip, bool rememberme)
        {
            bool isAuthSecure = await IsAuthRequestSecure(ip);
            if (!isAuthSecure)
            {
                await failedAttemptDAL.AddFailedAttempt(email, ip);
                return false;
            }
            bool isLocked = await IsAccountLocked(ip);
            if (isLocked)
                return false;

            UserAuthModel user = await authenticationDAL.GetUser(email);
            if (user.UserId == null)
            {
                await failedAttemptDAL.AddFailedAttempt(email, ip);
                return false;
            }
            if (user.Password == encrypt.HashPassword(password, user.Salt))
            {
                await this.Login((int)user.UserId!, rememberme);
                return true;
            }
            else
                await failedAttemptDAL.AddFailedAttempt(email, (int)user.UserId!, ip);

            return false;
        }

        public async Task<bool> IsAccountLocked(String email)
        {
            // в реальном приложении параметры должны конфигурироваться
            int emailAttemptsMinutes = -30;
            int emailThreshold = 3;
            int count = await failedAttemptDAL.GetFailedAttemptByEmail(email, emailAttemptsMinutes);
            if (count > emailThreshold)
                return true;
            return false;
        }

        public async Task<bool> IsAuthRequestSecure(String ip)
        {
            // в реальном приложении параметры должны конфигурироваться
            int ipAttemptsMinutes = -1;
            int ipThreshold = 3;

            int count = await failedAttemptDAL.GetFailedAttemptByIp(ip, ipAttemptsMinutes);
            if (count > ipThreshold)
                return false;
            return true;
        }

        public async Task<int> UpdatePassword(int userid, string salt, string password) {
            return await this.authenticationDAL.UpdatePassword(userid, encrypt.HashPassword(password, salt));
        }
    }
}

