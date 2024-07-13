using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyBlog.Controllers
{
    public class XsstestController : Controller
    {
        // GET: /<controller>/
        [Route("/xsstest")]
        public IActionResult Index(string id)
        {
            return View("Index", id);
        }

        [Route("/tagtest")]
        public IActionResult Tagtest(string id)
        {
            return View("tagtest", id);
        }

        [Route("/js")]
        public IActionResult Js(string id)
        {
            return View("js", id);
        }

        [Route("/xssaction")]
        public IActionResult Xssaction()
        {
            return Content("Hello " + 
                HttpContext.Request.Query["Test"], "text/html");
        }

        [Route("/xssaction2")]
        public IActionResult Xssaction2()
        {
            //var str = System.Security.SecurityElement.Escape(HttpContext.Request.Query["Test"]);
            var str = System.Text.Encodings.Web.HtmlEncoder.Default.Encode(HttpContext.Request.Query["Test"].ToString());
            return Content("Hello " + str, "text/html");
        }
    }
}

