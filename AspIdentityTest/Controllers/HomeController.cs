using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspIdentityTest.Models;
using Microsoft.AspNetCore.Identity;

namespace AspIdentityTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly SignInManager<IdentityUser> identityUser;

    public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> identityUser)
    {
        _logger = logger;
        this.identityUser = identityUser;
    }

    public IActionResult Index()
    {
        Console.WriteLine(User.Identity == null);
        return View();
    }

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
