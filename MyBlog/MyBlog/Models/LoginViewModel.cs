using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email обязательный")]
        [EmailAddress(ErrorMessage = "Неверный email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обязательный")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }

        public string? U { get; set; }
    }
}

