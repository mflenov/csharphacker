using System;
using Dapper;
using Microsoft.Data.SqlClient;
using MyBlog.BL.Auth;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Implementations.Dapper
{
    public class UserSecurityDAL: IUserSecurityDAL
    {
        public async Task<int> AddUserSecurity(UserSecurityModel model)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"insert into [UserSecurity] (UserId, VerificationCode)
                      values (@UserId, @VerificationCode)";

                return await connection.ExecuteAsync(sql, model);
            }
        }

        public async Task<UserSecurityModel> GetUserSecurity(int userid)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select UserId, VerificationCode from [UserSecurityModel] where userid = @userid";

                return await connection.QueryFirstAsync<UserSecurityModel>(sql, new { userid = userid });
            }
        }
    }
}

