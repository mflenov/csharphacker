
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using MyBlog.BL.Blog;
using MyBlog.BL.Auth;
using System.Security.Cryptography;
using MyBlog.Middleware;

namespace MyBlog.Controllers
{
    [BlogAuthorize(AuthRole.User)]
    public class BlogController : Controller
    {
        private readonly IBlog blog;
        private readonly ICurrentUser currentUser;

        public BlogController(IBlog blog, ICurrentUser currentUser)
        {
            this.blog = blog;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [Route("/blog/add")]
        public async Task<IActionResult> AddAction()
        {
            bool isLoggedIn = await currentUser.IsLoggedIn();
            if (isLoggedIn == false)
                return Redirect("/Login");
            return View("Edit", new BlogViewModel());
        }

        [HttpPost]
        [Route("/blog/add")]
        public async Task<IActionResult> AddPostAction(BlogViewModel model)
        {
            // Этот код можно отделить от контроллера,
            // но я оставлю здесь 
            string filename = "";

            var imagefiledata = this.Request.Form.Files["image"];
            if (imagefiledata != null)
            {
                MD5 md5hash = MD5.Create();

                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(imagefiledata.FileName);
                byte[] hashBytes = md5hash.ComputeHash(inputBytes);

                string hash = Convert.ToHexString(hashBytes);

                var dir = "./wwwroot/images/" + hash.Substring(0, 2) + "/" + hash.Substring(0, 4) ;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                filename = dir + "/" + imagefiledata.FileName;
                using (var stream = System.IO.File.Create(filename))
                {
                    await imagefiledata.CopyToAsync(stream);
                }

                filename = filename.Substring(9);
            }


            if (ModelState.IsValid)
            {
                var userdata = await currentUser.GetUserData();
                var blmodel = model.ToBlogModel();
                blmodel.UserId = userdata.UserId;
                blmodel.ImageFile = filename;
                await blog.Add(blmodel);
                return Redirect("/");
            }
            return View("Edit", model);
        }

        [HttpGet]
        [Route("/blog/edit")]
        public IActionResult EditAction(int id)
        {
            return View();
        }
    }
}

