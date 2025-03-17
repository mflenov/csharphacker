using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Controllers;

public class RedirectController : Controller
{

    [HttpGet]
    [Route("/redirect/index")]
    public IActionResult Index()
    {
        return View("Index");
    }

    [HttpGet]
    [Route("/redirect/toroute")]
    public IActionResult Toroute()
    {
        return RedirectToAction(nameof(HomeController.Index), typeof(HomeController).Name);
    }

    [HttpGet]
    [Route("/redirect/tourlv1")]
    public IActionResult tourlv1(string url)
    {
        return Redirect(url);
    }

    [HttpGet]
    [Route("/redirect/tourlv2")]
    public IActionResult tourlv2(string url)
    {
        if (!url.StartsWith("http"))
            return Redirect(url);
        return Redirect("/");
    }

    [HttpGet]
    [Route("/redirect/tourlv3")]
    public IActionResult tourlv3(string url)
    {
        if (!url.StartsWith("http") && !url.StartsWith("//"))
            return Redirect(url);
        return Redirect("/");
    }

    [HttpGet]
    [Route("/redirect/tourlv4")]
    public IActionResult tourlv4(string url)
    {
        if (Url.IsLocalUrl(url))
            return Redirect(url);
        return Redirect("/");
    }
}