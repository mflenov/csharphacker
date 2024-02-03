using System;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Implementations.Dapper;

namespace MyBlog.BL.Email
{
    public static class EmailProcessor
    {
        public static IEmailQueueDAL queue = new EmailQueueDAL();
        public static IEmailClient emailClient = new EmailClient("smtp.gmail.com", "noreply@flenov.ru", "пароль");

        public static void Process(int emailslimit)
        {
            var emails = queue.DeQueue(emailslimit).GetAwaiter().GetResult();
            foreach (var email in emails)
            {
                try
                {
                    emailClient.SendEmail(email.EmailTo!, email.EmailFrom!, email.EmailSubject!, email.EmailBody!);
                    queue.Delete(email.EmailQueueId).GetAwaiter().GetResult();
                }
                catch (Exception) {
                    queue.Retry(email.EmailQueueId).GetAwaiter().GetResult();
                }
            }
        }
    }
}

