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
    }
}

