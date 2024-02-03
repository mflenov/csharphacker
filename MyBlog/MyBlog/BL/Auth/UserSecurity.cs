using System;
using MyBlog.BL.Email;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
    public class UserSecurity: IUserSecurity
    {
        protected readonly IUserSecurityDAL userSecurityDAL;
        private readonly IEmailQueue emailqueue;

        public UserSecurity(IUserSecurityDAL userSecurityDAL,
                        IEmailQueue emailqueue)
        {
            this.userSecurityDAL = userSecurityDAL;
            this.emailqueue = emailqueue;
        }

        public async Task<int> CreateUserVerification(int userid, string email)
        {
            UserSecurityModel model = new UserSecurityModel();
            model.UserId = userid;
            model.VerificationCode = Guid.NewGuid().ToString();
            int id = await userSecurityDAL.AddUserSecurity(model);
            await emailqueue.EnqueMessage(email, "Account verification", @$"
                 Please confirm your email http://localhost/account/verification/{model.VerificationCode}
                ");
            return id;
        }

        public async Task<UserSecurityModel> GetUserSecurity(int userid)
        {
            return await userSecurityDAL.GetUserSecurity(userid);
        }
    }
}

