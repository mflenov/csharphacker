using System;
using Microsoft.Data.SqlClient;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Implementations.Ado
{
    public class FailedAttemptDAL: IFailedAttemptDAL
    {
        public async Task<int> AddFailedAttempt(string email, int userid, string ip)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"insert into [FailedAttempt] (Email, UserId, Created, IP)
                      values (@email, @userid, GetDate(), @ip)";
                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.Add(new SqlParameter("email", email));
                command.Parameters.Add(new SqlParameter("userid", userid));
                command.Parameters.Add(new SqlParameter("ip", ip));

                var id = await command.ExecuteNonQueryAsync();
                return id;
            }
        }

        public async Task<int> AddFailedAttempt(string email, string ip)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"insert into [FailedAttempt] (Email, Created, Ip)
                      values (@email, GetDate(), @ip)";
                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.Add(new SqlParameter("email", email));
                command.Parameters.Add(new SqlParameter("ip", ip));

                var id = await command.ExecuteNonQueryAsync();
                return id;
            }
        }
        public async Task<int> GetFailedAttemptByEmail(string email, int minutes)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select count(*)
                        from FailedAttempt
                        where Email = @email and Created > dateadd(MINUTE, @m, getdate())";
                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.Add(new SqlParameter("email", email));
                command.Parameters.Add(new SqlParameter("m", minutes));
                var count = await command.ExecuteScalarAsync();
                return await Task.FromResult<int>((int)count!);
            }
        }

        public async Task<int> GetFailedAttemptByIp(string ip, int minutes)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select count(*)
                        from FailedAttempt
                        where Ip = @ip and Created > dateadd(MINUTE, @m, getdate())";
                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.Add(new SqlParameter("ip", ip));
                command.Parameters.Add(new SqlParameter("m", minutes));
                var count = await command.ExecuteScalarAsync();
                return await Task.FromResult<int>((int)count!);
            }
        }
    }
}

