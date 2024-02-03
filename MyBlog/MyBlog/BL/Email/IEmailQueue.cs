using System;
namespace MyBlog.BL.Email
{
    public interface IEmailQueue
    {
        Task<int> EnqueMessage(string email, string subject, string body);
    }
}

