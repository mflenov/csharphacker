using Dapper;
using Microsoft.Data.SqlClient;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Implementations.Dapper;

public class UserTokenDAL : Interfaces.IUserTokenDAL
{
    public async Task<Guid> Create(UserTokenModel model)
    {
        using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
        {
            string sql = @"insert into UserToken (UserTokenID, UserId, Created, UserAgent)
                    values (@UserTokenId, @userid, getdate(), @userAgent)";

            await connection.ExecuteAsync(sql, model);
            return (Guid)model.UserTokenId!;
        }
    }

    public async Task<int?> Get(Guid tokenid)
    {
        using (var connection = new SqlConnection(DbHelper.GetConnectionString()))
        {
            string sql = @"select UserId from UserToken where UserTokenID = @tokenid";
            return await connection.QueryFirstOrDefaultAsync<int?>(sql, new { tokenid = tokenid });
        }
    }
}
