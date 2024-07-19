using System.ComponentModel.DataAnnotations;
using MyBlog.DAL.Models;

namespace MyBlog.Models
{
    public class BlogViewModel
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Слишком длиный заголовок")]
        public string? Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? Content { get; set; }

        public BlogModel ToBlogModel()
        {
            return new BlogModel()
            {
                Title = this.Title,
                Content = this.Content
            };
        }
    }
}

