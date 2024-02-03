using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Implementations.EF
{
    public class RawEfAuthenticationDAL: IAuthenticationDAL
    {
        public async Task<UserModel> GetUser(int id)
        {
            using (var dbContext = new EfContext())
            {
                return await dbContext.User.Where(m => m.UserId == id).FirstOrDefaultAsync() ?? new UserModel();
            }
        }

        public async Task<UserAuthModel> GetUser(string email)
        {
            using (var dbContext = new EfContext())
            {
                var emailParam = new SqlParameter("email", email);
                var result = await dbContext.User.FromSqlRaw("select * from [User] where Email = @email", emailParam).ToListAsync();
//                var result = dbContext.User.FromSqlRaw("select * from [User] where Email = '" + email + "'").ToList();
                if (result.Count > 0)
                    return result
                    .Select( m => new UserAuthModel()
                        {
                            UserId = m.UserId,
                            Email = m.Email,
                            Password = m.Password,
                            Salt = m.Salt
                        }
                    ).FirstOrDefault()!;
                return new UserAuthModel();
            }
        }

        public async Task<int> CreateUser(UserModel user)
        {
            using (var dbContext = new EfContext())
            {
                dbContext.User.Add(user);
                return await dbContext.SaveChangesAsync();
            }
        }

        public async Task<UserAuthModel> GetUserByNormalizedEmail(string email)
        {
            using (var dbContext = new EfContext())
            {
                var emailParam = new SqlParameter("email", email);
                var result = await dbContext.User.FromSqlRaw("select * from [User] where NormilizedEmail = @email", emailParam).ToListAsync();
                //                var result = dbContext.User.FromSqlRaw("select * from [User] where NormilizedEmail = '" + email + "'").ToList();
                if (result.Count > 0)
                    return result
                    .Select(m => new UserAuthModel()
                    {
                        UserId = m.UserId,
                        Email = m.Email,
                        Password = m.Password,
                        Salt = m.Salt,
                        NormilizedEmail = m.NormilizedEmail
                    }
                    ).FirstOrDefault()!;
                return new UserAuthModel();
            }
        }
        public Task<int> UpdatePassword(int userid, string password)
        {
            throw new Exception();
        }
    }
}

