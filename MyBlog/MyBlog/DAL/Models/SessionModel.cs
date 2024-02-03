using System;
namespace MyBlog.DAL.Models
{
    public class SessionModel
    {
        public Guid SessionId { get; set; }

        public string? Content { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastAccessed { get; set; }

        public int? UserId { get; set; }
    }
}

