using System;
using Microsoft.Data.SqlClient;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace MyBlog.DAL.Implementations.EF
{
    public class EfAuthenticationDALL: IAuthenticationDAL
    {
        public async Task<UserModel> GetUser(int id)
        {
            using (var dbContext = new EfContext())
                return await dbContext.User.Where(m => m.UserId == id).FirstOrDefaultAsync() ?? new UserModel();
        }

        public async Task<UserAuthModel> GetUser(string email)
        {
            using (var dbContext = new EfContext())
                return await dbContext.User.Where(m => m.Email == email).Select(
                    m => new UserAuthModel()
                    {
                        UserId = m.UserId,
                        Email = m.Email,
                        Password = m.Password,
                        Salt = m.Salt
                    }
                    )
                    .FirstOrDefaultAsync() ?? new UserAuthModel();
        }


        public async Task<UserAuthModel> GetUserByNormalizedEmail(string email)
        {
            using (var dbContext = new EfContext())
                return await dbContext.User.Where(m => m.NormilizedEmail == email).Select(
                    m => new UserAuthModel()
                    {
                        UserId = m.UserId,
                        Email = m.Email,
                        Password = m.Password,
                        Salt = m.Salt,
                        NormilizedEmail = m.NormilizedEmail
                    }
                    )
                    .FirstOrDefaultAsync() ?? new UserAuthModel();
        }

        public async Task<int> CreateUser(UserModel user)
        {
            using (var dbContext = new EfContext())
            {
                dbContext.User.Add(user);
                return await dbContext.SaveChangesAsync();
            }
        }
        public Task<int> UpdatePassword(int userid, string password)
        {
            throw new Exception();
        }
    }
}

