using System;
using System.Net.NetworkInformation;
using Dapper;
using Microsoft.Data.SqlClient;
using MyBlog.BL.Auth;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;


namespace MyBlog.DAL.Implementations.Dapper
{
    public class SessionDAL: ISessionDAL
    {
        public async Task<int> CreateSession(SessionModel model)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"insert into [Session] (SessionID, Content, Created, LastAccessed, UserId)
                      values (@SessionID, @Content, @Created, @LastAccessed, @UserId)";

                return await connection.ExecuteAsync(sql, model);
            }
        }

        public async Task<SessionModel?> GetSession(Guid sessionId)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"select SessionID, Content, Created, LastAccessed, UserId  from [Session] where sessionId = @sessionId";

                var sessions = await connection.QueryAsync<SessionModel>(sql, new { sessionId = sessionId });
                return sessions.FirstOrDefault();
            }
        }

        public async Task<int> UpdateSession(SessionModel model)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                await connection.OpenAsync();
                string sql = @"update [Session]
                      set Content = @Content
                      where SessionID = @SessionID
                ";

                return await connection.ExecuteAsync(sql, model);
            }
        }

        public async Task Extend(Guid dbSessionID)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                string sql = @"update DbSession
                        set LastAccessed = @lastAccessed
                        where DbSessionID = @dbSessionID";

                await connection.ExecuteAsync(sql, new { dbSessionID = dbSessionID, lastAccessed = DateTime.Now });
            }
        }

        public async Task Delete(Guid sessionId)
        {
            using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
            {
                string sql = @"delete from DbSession where DbSessionID = @dbSessionID";

                await connection.ExecuteAsync(sql, new { dbSessionID = sessionId });
            }
        }                
    }
}

