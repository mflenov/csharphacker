using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Controllers;

public class CspController : Controller
{
    [Route("/csp")]
    public IActionResult Index()
    {
        return View("Index");
    }

    [Route("/csp/header")]
    public IActionResult Header()
    {
        HttpContext.Response.Headers.Append(
            "Content-Security-Policy", 
            "default-src 'self' www.flenov.info"
        );
        return View("csp");
    }
}
