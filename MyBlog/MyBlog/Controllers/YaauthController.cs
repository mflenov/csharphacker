using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Controllers;

public class YaauthController : Controller {
    private readonly IHttpContextAccessor httpContextAccessor;

    static readonly HttpClient client = new HttpClient();

    public YaauthController(IHttpContextAccessor httpContextAccessor) {
        this.httpContextAccessor = httpContextAccessor;
    }

    [Route("/yaauth")]
    public async Task<ActionResult> Index(string state, string code) {
        string sessionstate = httpContextAccessor?.HttpContext?.Session.GetString("authcode") ?? "sdfsdf";
        if (state != sessionstate) {
            return new ContentResult { Content = "Ошибка безопасности" };
        }

        var form = new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "code", code },
                    { "client_id", Models.YaAuthViewModel.Clientid },
                    { "client_secret", Models.YaAuthViewModel.ClientSecret }
                };

        client.DefaultRequestHeaders.Clear();
        HttpResponseMessage tokenResponse = await client.PostAsync("https://oauth.yandex.ru/token",new FormUrlEncodedContent(form));
        var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
        Models.YaTokenViewModel? token = JsonSerializer.Deserialize<Models.YaTokenViewModel>(tokenContent); 

        if (token == null || !String.IsNullOrEmpty(token.error)) {
            return new ContentResult { Content = "Ошибка " +  token?.error};
        }

        client.DefaultRequestHeaders.Add("Authorization", "OAuth " + token?.access_token );
        HttpResponseMessage infoResponse = await client.PostAsync("https://login.yandex.ru/info", null);
        var infoContent = await infoResponse.Content.ReadAsStringAsync();
        Models.YaInfoViewModel? userInfo = JsonSerializer.Deserialize<Models.YaInfoViewModel>(infoContent); 

        return new ContentResult { Content = tokenContent };
    }
}