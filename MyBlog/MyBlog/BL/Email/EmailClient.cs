using System.Net;
using System.Net.Mail;

namespace MyBlog.BL.Email
{
    public class EmailClient: IEmailClient
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string username = "username";
        private readonly string password = "password";

        public EmailClient(string smtpServer, string username, string password, int smtpPort = 587)
        {
            this.smtpServer = smtpServer;
            this.username = username;
            this.password = password;
            this.smtpPort = smtpPort;
        }

        public void SendEmail(string to, string from, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = smtpServer;
            smtpClient.Port = smtpPort;
            smtpClient.EnableSsl = true;
            smtpClient.Timeout = 10000;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(username, password);

            var message = new MailMessage(new MailAddress(from), new MailAddress(to));
            message.Subject = subject;
            message.Body = body;

            smtpClient.Send(message);
        }

    }
}

