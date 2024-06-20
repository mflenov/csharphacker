using MyBlog.DAL.Models;

namespace MyBlog.BL.Auth
{
    public interface ICurrentUser
    {
        Task<bool> IsLoggedIn();

        Task<UserModel> GetUserData();
    }
}

