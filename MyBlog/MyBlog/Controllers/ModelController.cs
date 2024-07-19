using Microsoft.AspNetCore.Mvc;
using MyBlog.DAL.Models;

namespace MyBlog.Controllers
{
    public class ModelController : Controller
    {
        [HttpGet]
        [Route("/model/add")]
        public IActionResult AddAction()
        {
            return View("Add", new BlogModel());
        }

        [HttpPost]
        [Route("/model/add")]
        public IActionResult AddAction([Bind("Title, Content")]BlogModel model)
        {
            return View("Add", model);
        }
    }
}