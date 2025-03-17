namespace MyBlog.DAL.Models
{
    public class UserTokenModel
    {
        public Guid? UserTokenId { get; set; }

        public int? UserId { get; set; }

        public DateTime Created { get; set;}

        public string? UserAgent { get; set; }
    }
}

