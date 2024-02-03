using System;

namespace MyBlog.DAL.Models
{
    public class FailedAttemptModel
    {
        public long? FailedAttemptModelId { get; set; }

        public string? Email { get; set; }

        public int? UserId { get; set; }

        public DateTime? Created { get; set; }
    }
}

