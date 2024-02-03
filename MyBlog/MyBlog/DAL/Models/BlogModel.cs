using System;

namespace MyBlog.DAL.Models
{
    public class BlogModel
    {
        public int BlogId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? Created { get; set; }
        public int? Status { get; set; }
        public int? UserId { get; set; }
        public string? ImageFile { get; set; }
    }
}

