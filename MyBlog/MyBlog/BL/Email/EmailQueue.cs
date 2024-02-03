using System;
using MyBlog.DAL.Interfaces;

namespace MyBlog.BL.Email
{
    public class EmailQueue : IEmailQueue
    {
        private readonly IEmailQueueDAL emailQueueDAL;

        public EmailQueue(IEmailQueueDAL emailQueueDAL)
        {
            this.emailQueueDAL = emailQueueDAL;
        }

        public string From { get; set; } = "noreply@flenov.com";

        public async Task<int> EnqueMessage(string email, string subject, string body)
        {
            return await emailQueueDAL.Queue(
                new DAL.Models.EmailQueueModel()
                {
                    EmailFrom = From,
                    EmailTo = email,
                    EmailSubject = subject,
                    EmailBody = body,
                    Created = DateTime.Now
                }
            );
        }
    }
}

