using MyBlog.DAL.Models;

namespace MyBlog.DAL.Interfaces
{
    public interface IEmailQueueDAL
    {
        Task<int> Queue(EmailQueueModel model);

        Task<IEnumerable<EmailQueueModel>> DeQueue(int emailslimit);

        Task<int> Delete(int emailQueueId);

        Task<int> Retry(int emailQueueId);
    }
}

