using System;
using Dapper;
using Microsoft.Data.SqlClient;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Implementations.Dapper
{
    public class DapperAuthenticationDAL: IAuthenticationDAL
    {
        public async Task<UserModel> GetUser(int id)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select UserId, Salt, Email, Password, FirstName, LastName, ProfileImage
                      from [User]
                      where userid = @id";
                var userModel = await connection.QueryFirstAsync<UserModel>(sql, new { id = id });
                return userModel ?? new UserModel();
            }
        }

        public async Task<UserAuthModel> GetUser(string email)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select UserId, Email, Password, Salt
                      from [User]
                      where email = @email";

                var userModel = await connection.QueryFirstOrDefaultAsync<UserAuthModel>(sql, new { email = email });
                return userModel ?? new UserAuthModel();
            }
        }

        public async Task<int> CreateUser(UserModel user)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"insert into [User] (Email, Password, Salt, NormilizedEmail)
                      values (@Email, @Password, @Salt, @NormilizedEmail);
                        SELECT SCOPE_IDENTITY()";
                return await connection.ExecuteScalarAsync<int>(sql, user);
            }
        }

        public async Task<UserAuthModel> GetUserByNormalizedEmail(string email)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select UserId, Email, Password, Salt
                      from [User]
                      where NormilizedEmail = @email";

                var userModel = await connection.QueryFirstOrDefaultAsync<UserAuthModel>(sql, new { email = email });
                return userModel ?? new UserAuthModel();
            }
        }

        public async Task<int> UpdatePassword(int userid, string password)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"update [User]
                    set Password = @p
                    where UserId = @id";

                return await connection.ExecuteAsync(sql, new { p = password, id = userid });
            }

        }
    }
}
