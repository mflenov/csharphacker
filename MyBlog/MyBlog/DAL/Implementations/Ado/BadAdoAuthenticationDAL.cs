using System;
using MyBlog.DAL.Models;
using Microsoft.Data.SqlClient;
using MyBlog.DAL.Interfaces;

namespace MyBlog.DAL.Implementations
{
    public class BadAdoAuthenticationDAL : IAuthenticationDAL
    {
        public async Task<UserModel> GetUser(int id)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select UserId, Email, Password, FirstName, LastName, ProfileImage
                      from [User]
                      where userid = " + id;

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = await command.ExecuteReaderAsync();
                UserModel user = new UserModel();
                if (await reader.ReadAsync())
                {
                    user.UserId = reader.GetInt32(0);
                    user.Email = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.FirstName = reader.GetString(3);
                    user.LastName = reader.GetString(4);
                    user.ProfileImage = reader.GetString(5);
                }
                return user;
            }
        }

        public async Task<UserAuthModel> GetUser(string email)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select UserId, Email, Password, Salt
                      from [User]
                      where email = '" + email + "'";

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = await command.ExecuteReaderAsync();
                UserAuthModel user = new UserAuthModel();
                if (await reader.ReadAsync())
                {
                    user.UserId = reader.GetInt32(0);
                    user.Email = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.Salt = reader.GetString(3);
                }
                return user;
            }
        }

        public async Task<int> CreateUser(UserModel user)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"insert into [User] (Email, Password, Salt, NormilizedEmail)
                      values ('" + user.Email + "', '" + user.Password + "', '" + user.Salt + "', '" + user.NormilizedEmail + "')";
                SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.Add(new SqlParameter("Email", user.Email));
                command.Parameters.Add(new SqlParameter("Password", user.Password));
                command.Parameters.Add(new SqlParameter("Salt", user.Salt));
                command.Parameters.Add(new SqlParameter("NormilizedEmail", user.NormilizedEmail));

                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<UserAuthModel> GetUserByNormalizedEmail(string email)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select UserId, Email, Password, Salt
                      from [User]
                      where NormilizedEmail = '" + email + "'";

                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = await command.ExecuteReaderAsync();
                UserAuthModel user = new UserAuthModel();
                if (await reader.ReadAsync())
                {
                    user.UserId = reader.GetInt32(0);
                    user.Email = reader.GetString(1);
                    user.Password = reader.GetString(2);
                    user.Salt = reader.GetString(3);
                }
                return user;
            }
        }
        public Task<int> UpdatePassword(int userid, string password)
        {
            throw new Exception();
        }
    }
}

