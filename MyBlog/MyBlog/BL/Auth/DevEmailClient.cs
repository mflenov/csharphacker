using System;
using MyBlog.BL.Email;

namespace MyBlog.BL.Auth
{
    public class DevEmailClient : IEmailClient
    {
        public DevEmailClient()
        {
        }

        public void SendEmail(string to, string from, string subject, string body)
        {
            throw new NotImplementedException();
        }
    }
}

