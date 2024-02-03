using System;
namespace MyBlog.DAL.Models
{
    public class UserSecurityModel
    {
        public int? UserSecurityId { get; set; }

        public int? UserId { get; set; }

        public string? VerificationCode { get; set; }
    }
}

