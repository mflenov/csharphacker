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

    [Route("/csp/headereval1")]
    public IActionResult Headereval1()
    {
        HttpContext.Response.Headers.Append(
            "Content-Security-Policy", 
            "default-src 'self' www.flenov.info 'unsafe-eval' 'unsafe-inline'" 
        );
        return View("cspeval");
    }

    [Route("/csp/headereval2")]
    public IActionResult Headereval2()
    {
        HttpContext.Response.Headers.Append(
            "Content-Security-Policy-Report-Only", 
            "default-src 'self'; report-uri /csp/csperrors" 
        );
        return View("cspeval");
    }

    [Route("/csp/csperrors")]
    public async Task<IActionResult> csperrors() {
        var report = "";

        using (var reader = new StreamReader(Request.Body))
            report = await reader.ReadToEndAsync();
                    
        return new ContentResult() { Content = report };
    }
}
