namespace MyBlog.DAL.Models
{
    public class UserAuthModel
    {
        public int? UserId { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Salt { get; set; } = null!;

        public string? NormilizedEmail { get; set; }
    }
}

