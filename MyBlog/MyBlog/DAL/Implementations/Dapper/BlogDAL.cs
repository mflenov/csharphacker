using System;
using MyBlog.DAL.Models;
using MyBlog.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;

namespace MyBlog.DAL.Implementations.Dapper
{
    public class BlogDAL : IBlogDAL
    {
        public async Task<int> Add(BlogModel model)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"insert into [Blog] (Title, Content, Created, UserId, [Status], ImageFile)
                      values (@Title, @Content, @Created, @UserId, @Status, @ImageFile)";

                return await connection.ExecuteAsync(sql, model);
            }
        }

        public async Task<IEnumerable<BlogModel>> List(string status)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select BlogId, Title, Content, Created, UserId, [Status], ImageFile from [Blog] where Status = @status";

                return await connection.QueryAsync<BlogModel>(sql, new { status = status });
            }
        }

        public Task<int> Update(BlogModel model)
        {
            throw new NotImplementedException();
        }
    }
}

