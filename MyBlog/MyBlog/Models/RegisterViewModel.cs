using System;
using System.ComponentModel.DataAnnotations;
using MyBlog.DAL.Models;
using MyBlog.BL.Auth;

namespace MyBlog.Models
{
    public class RegisterViewModel: IValidatableObject
    {
        [Required(ErrorMessage = "Email обязательный")]
        [EmailAddress(ErrorMessage = "Неверный email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обязательный")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*-]).{10,}$",
                ErrorMessage = "Пароль слишком простой")]
        public string? Password { get; set; }

        public UserModel ToUserModel()
        {
            return new UserModel()
            {
                Email = this.Email!,
                Password = this.Password!
            };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password == "Qwert!2345")
            {
                yield return new ValidationResult("Пароль слишком простой", new [] { "Password" });
            }
        }
    }
}

