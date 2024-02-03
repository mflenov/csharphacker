using Dapper;
using Microsoft.Data.SqlClient;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Implementations.Dapper
{
    public class EmailQueueDAL : IEmailQueueDAL
    {
        public async Task<int> Queue(EmailQueueModel model)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"insert into [EmailQueue] (EmailTo, EmailFrom, EmailSubject, EmailBody, Created, Retry)
                      values (@EmailTo, @EmailFrom, @EmailSubject, @EmailBody, @Created, 0)";

                return await connection.ExecuteAsync(sql, model);
            }
        }
        public async Task<IEnumerable<EmailQueueModel>> DeQueue(int emailslimit)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                Guid guid = Guid.NewGuid();

                // в следующем примере интерполяцией вставляется значение emailslimit
                // а как же параметры? Да, это плохо, но я просто уверен, что emailslimit
                // это число и оно не может стать причиной SQL инъекции, поэтому тут
                // средал угол 
                await connection.ExecuteAsync(@$"update EmailQueue
                      set ProcessingId = @id
                      where EmailQueueId in (
                         select top {emailslimit} EmailQueueId
                         from EmailQueue
                         where ProcessingId is null and Retry < 5)",
                    new { id = guid });

                return await connection.QueryAsync<EmailQueueModel>(@"
                      select EmailQueueId, EmailTo, EmailFrom, EmailSubject, EmailBody, Created, ProcessingId
                      from EmailQueue", new { id = guid });
            }
        }

        public async Task<int> Delete(int emailQueueId)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(@"
                      delete from EmailQueue where EmailQueueId = @id
                ", new { id = emailQueueId });
            }
        }

        public async Task<int> Retry(int emailQueueId)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();

                return await connection.ExecuteAsync(@"
                      update EmailQueue set ProcessingId = null, Retry = Retry + 1 where EmailQueueId = @id
                ", new { id = emailQueueId });
            }
        }
    }
}

