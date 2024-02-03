using System;
namespace MyBlog.DAL.Models
{
    public class EmailQueueModel
    {
        public int EmailQueueId { get; set; }

        public string? EmailTo { get; set; }

        public string? EmailFrom { get; set; }

        public string? EmailSubject { get; set; }

        public string? EmailBody { get; set; }

        public string? ProcessingId { get; set; }

        public DateTime Created { get; set; }

        public int? Retry { get; set; }
    }
}

