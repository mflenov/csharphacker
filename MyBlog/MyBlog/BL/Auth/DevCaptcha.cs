using System;

namespace MyBlog.BL.Auth
{
    public class DevCaptcha : ICaptcha
    {
        public DevCaptcha()
        {
        }

        public string GetSitekey()
        {
            return "";
        }

        public Task<bool> ValidateToken(string token)
        {
            return Task.FromResult<bool>(true);
        }
    }
}

