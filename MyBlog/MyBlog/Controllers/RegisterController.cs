using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyBlog.BL.Auth;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IAuthentication authentication;
        private readonly IUser user;
        private readonly ICaptcha captcha;

        public RegisterController(IAuthentication authentication, IUser user, ICaptcha captcha)
        {
            this.authentication = authentication;
            this.user = user;
            this.captcha = captcha;
        }

        [HttpGet]
        [Route("/Register")]
        public IActionResult Index()
        {
            ViewBag.CaptchaSitekey = captcha.GetSitekey();
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        [Route("/Register")]
        public async Task<IActionResult> IndexPost(RegisterViewModel model)
        {
            ViewBag.CaptchaSitekey = captcha.GetSitekey();
            bool isCaptchaValid = await captcha.ValidateToken(Request.Form["g-recaptcha-response"]!);
            if (isCaptchaValid)
            {

                var emailError = await authentication.ValidateEmail(model.Email);
                if (emailError != null)
                    ModelState.TryAddModelError("Email", emailError.ErrorMessage!);

                if (ModelState.IsValid)
                {
                    await authentication.CreateUser(model.ToUserModel());
                    return Redirect("/");
                }
            }
            else
            {
                ModelState.TryAddModelError("captcha", "Incorrect Captcha");
            }
            return View("Index", model);
        }
    }
}

