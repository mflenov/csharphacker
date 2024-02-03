namespace MyBlog.BL.Auth
{
    public interface ICaptcha
    {
        string GetSitekey();

        Task<bool> ValidateToken(string token);
    }
}

