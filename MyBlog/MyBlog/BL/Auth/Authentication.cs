using System.ComponentModel.DataAnnotations;
using MyBlog.BL.Auth;
using MyBlog.BL.Email;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
    public class Authentication: IAuthentication
    {
        private readonly IAuthenticationDAL authenticationDAL;
        private readonly IFailedAttemptDAL failedAttemptDAL;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IEncrypt encrypt;
        private readonly IUserSecurity userSecurity;
        private readonly ISession session;

        public Authentication(IAuthenticationDAL authenticationDAL,
            IFailedAttemptDAL failedAttemptDAL,
            IHttpContextAccessor httpContextAccessor,
            IEncrypt encrypt,
            ISession session,
            IUserSecurity userSecurity)
        {
            this.authenticationDAL = authenticationDAL;
            this.failedAttemptDAL = failedAttemptDAL;
            this.httpContextAccessor = httpContextAccessor;
            this.encrypt = encrypt;
            this.userSecurity = userSecurity;
            this.session = session;
        }

        public async Task<int> CreateUser(UserModel user)
        {
            user.Salt = Guid.NewGuid().ToString();
            user.Password = encrypt.HashPassword(user.Password, user.Salt);
            user.NormilizedEmail = MyBlog.BL.General.BlHelpers.NormilizeEmail(user.Email);

            int id = await authenticationDAL.CreateUser(user);
            await userSecurity.CreateUserVerification(id, user.Email);
            this.Login(id, false);

            return id;
        }

        public void Login(int id, bool rememberme)
        {
            httpContextAccessor?.HttpContext?.Session.SetInt32("userid", id);
            //опять же это плохо использовать GetAwaiter().GetResult();
            // но это что-бы сохранить код
            //this.session.SetUserId(id).GetAwaiter().GetResult();

            if (rememberme)
            {
                CookieOptions options = new CookieOptions();
                options.Path = "/";
                options.HttpOnly = true;
                options.Secure = true;
                options.Expires = DateTimeOffset.UtcNow.AddDays(30);
                httpContextAccessor?.HttpContext?.Response.Cookies.Append(General.Constants.RememberMeCookieName, encrypt.Encrypt(id.ToString()), options);
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
                this.Login((int)user.UserId!, rememberme);
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

