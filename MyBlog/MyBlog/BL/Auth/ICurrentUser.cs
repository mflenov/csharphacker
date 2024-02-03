using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
    public interface ICurrentUser
    {
        bool IsLoggedIn();

        Task<UserModel> GetUserData();
    }
}

