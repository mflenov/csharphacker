namespace MyBlog.DAL.Interfaces;

public interface IUserTokenDAL
{
    Task<Guid> Create(Models.UserTokenModel model);

    Task<int?> Get(Guid tokenid);

    Task DeleteToken(Guid tokenid);
}


