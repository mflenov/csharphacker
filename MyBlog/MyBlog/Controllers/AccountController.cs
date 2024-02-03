using System;
using Microsoft.AspNetCore.Mvc;
using MyBlog.BL.Auth;
using MyBlog.BL.Blog;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController: Controller
    {
        private readonly ICurrentUser currentUser;
        private readonly IAuthentication authentication;

        public AccountController(ICurrentUser currentUser, IAuthentication authentication)
        {
            this.currentUser = currentUser;
            this.authentication = authentication;
        }


        [HttpGet]
        [Route("/account/password")]
        public IActionResult AddAction()
        {
            if (!currentUser.IsLoggedIn())
                return Redirect("/Login");
            return View("Password", new PasswordViewModel());
        }

        [HttpPost]
        [Route("/account/password")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPostAction(PasswordViewModel model)
        {
            if (!currentUser.IsLoggedIn())
                return Redirect("/Login");

            if (ModelState.IsValid)
            {
                var userdata = await currentUser.GetUserData();
                await authentication.UpdatePassword((int)userdata.UserId!, userdata.Salt, model.NewPassword1);
                return Redirect("/");
            }
            return View("Password", model);
        }
    }
}

