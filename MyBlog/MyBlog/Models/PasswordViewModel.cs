using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Models
{
    public class PasswordViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Пароль обязательный")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[!@#$%^&*-]).{10,}$",
                ErrorMessage = "Пароль слишком простой")]
        public string NewPassword1 { get; set; } = "";


        [Required(ErrorMessage = "Пароль обязательный")]
        public string NewPassword2 { get; set; } = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NewPassword1 != NewPassword2)
            {
                yield return new ValidationResult("Пароли не совпадают", new[] { "NewPassword1" });
            }
        }
    }
}

