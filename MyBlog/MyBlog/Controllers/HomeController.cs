using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MyBlog.BL.Auth;
using MyBlog.BL.Blog;
using MyBlog.Models;

namespace MyBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ICurrentUser user;
    private readonly IBlog blog;
    private readonly IHttpClientFactory httpClientFactory;

    public HomeController(ILogger<HomeController> logger, ICurrentUser user, IBlog blog, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        this.user = user;
        this.blog = blog;
        this.httpClientFactory = httpClientFactory;
    }

    //[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
    [EnableRateLimiting("sliding")]
    public async Task<IActionResult> Index(string status = "0")
    {
        var httpClient = httpClientFactory.CreateClient();

        HomeViewModel model = new HomeViewModel();
        model.IsLoggedIn = await this.user.IsLoggedIn();
        model.BlogItems = await blog.List(status);
        return View(model);
    }

    [EnableRateLimiting("fixed")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

