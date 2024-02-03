using System;
using System.Net;
using System.Xml.Linq;
using System.Text.Json;

namespace MyBlog.BL.Auth
{
    public class GoogleTokenResult
    {
        public bool success { get; set; }
    }

    public class GoogleCaptcha: ICaptcha
    {
        private readonly string Sitekey;
        private readonly string Secret;
        HttpClient httpClient = new HttpClient();

        public GoogleCaptcha(string sitekey, string secret)
        {
            this.Sitekey = sitekey;
            this.Secret = secret;
        }

        public string GetSitekey()
        {
            return Sitekey;
        }


        public async Task<bool> ValidateToken(string token)
        {
            var res = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={Secret}&response={token}");

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            string json = await res.Content.ReadAsStringAsync();

            GoogleTokenResult? tocketresponse = JsonSerializer.Deserialize<GoogleTokenResult>(json);

            return tocketresponse?.success == true;
        }
    }
}

