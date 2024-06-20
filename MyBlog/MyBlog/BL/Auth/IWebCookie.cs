namespace MyBlog.BL.Auth
{
	public interface IWebCookie
	{
        public void AddSecure(string cookieName, string value, int days = 0);

        void Add(string cookieName, string value, int days = 0);

		void Delete(string cookieName);

        string? Get(string cookieName);
    }
}

