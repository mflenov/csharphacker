using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.DAL.Models
{
    public class UserModel
    {
        [Key]
        public int? UserId { get; set; }

        public string Email { get; set; } = null!;

        public string? NormilizedEmail { get; set; }

        public string Password { get; set; } = null!;

        public string Salt { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ProfileImage { get; set; }

        public int? Status { get; set; }
    }
}

