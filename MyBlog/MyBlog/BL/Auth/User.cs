using MyBlog.DAL.Interfaces;

namespace MyBlog.BL.Auth
{
    public class User: IUser
    {

        public User(IAuthenticationDAL authenticationDAL)
        {
        }
    }
}

