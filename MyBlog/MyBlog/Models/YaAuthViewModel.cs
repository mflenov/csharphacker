
namespace MyBlog.Models;

public class YaAuthViewModel
{
    public static string Clientid = "fe4c39d8d6384fe98b3053488a08ed22";

    public static string ClientSecret = "ebd391b8e8344e04a075932cf2990503";

    public static string GetAuthUrl(IHttpContextAccessor httpContextAccessor) {
        string authcode = Guid.NewGuid().ToString();
        httpContextAccessor?.HttpContext?.Session.SetString("authcode", authcode);
        string returnUrl = Uri.EscapeDataString("https://test.flenov.ru:37741/yaauth");
        
        return $"response_type=code&redirect_uri={returnUrl}&client_id={Clientid}&state={authcode}";
    }
}

public class YaTokenViewModel {
    public string? access_token { get; set; }
    public int? expires_in { get; set; }
    public string? refresh_token { get; set; }
    public string? token_type { get; set; }
    public string? error { get; set; }
    public string? error_description { get; set; }
}

public class YaInfoViewModel {

	public string? first_name { get; set; }
	public string? last_name { get; set; }
	public string? display_name { get; set; }
	public List<string> emails { get; set; } = new List<string>();
    public string? default_avatar_id { get; set; }
	public string? default_email { get; set; }
	public string? real_name { get; set; }
	public bool? is_avatar_empty { get; set; }
	public string? client_id { get; set; }
	public string? login { get; set; }
	public string? sex { get; set; }
	public string? id { get; set; }
}

