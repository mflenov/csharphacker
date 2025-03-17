namespace MyBlog.Service
{
    public class WebCookie : BL.Auth.IWebCookie
    {
        IHttpContextAccessor httpContextAccessor;

        public WebCookie(IHttpContextAccessor httpContextAccessor)
		{
            this.httpContextAccessor = httpContextAccessor;
        }

        public void AddSecure(string cookieName, string value, int days = 0)
        {
            CookieOptions options = new CookieOptions();
            options.Path = "/";
            options.HttpOnly = true;
            options.Secure = true;
            if (days > 0)
                options.Expires = DateTimeOffset.UtcNow.AddDays(days);
            httpContextAccessor?.HttpContext?.Response.Cookies.Append(cookieName, value, options);
        }

        public void Add(string cookieName, string value, int days = 0)
        {
            CookieOptions options = new CookieOptions();
            options.Path = "/";
            if (days > 0)
                options.Expires = DateTimeOffset.UtcNow.AddDays(days);
            httpContextAccessor?.HttpContext?.Response.Cookies.Append(cookieName, value, options);
        }

        public void Delete(string cookieName)
        {
            httpContextAccessor?.HttpContext?.Response.Cookies.Delete(cookieName);
        }

        public string? Get(string cookieName)
        {
            var cookie = httpContextAccessor?.HttpContext?.Request?.Cookies.FirstOrDefault(m => m.Key == cookieName);
            if (cookie != null && cookie.Value.Value != null)
                return cookie.Value.Value;
            return null;
        }
    }
}

