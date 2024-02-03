using Microsoft.EntityFrameworkCore;
using MyBlog.DAL.Models;

namespace MyBlog.DAL.Implementations.EF
{
    public class EfContext : DbContext
    {
        public virtual DbSet<UserModel> User { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
                    .UseSqlServer("Data Source=.;Initial Catalog=hackishssharp;uid=sa;pwd=Hujkq23&6#;Trust Server Certificate=true;");
            }
        }
    }
}

