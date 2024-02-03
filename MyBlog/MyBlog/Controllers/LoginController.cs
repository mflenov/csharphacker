using System;
using System.Collections.Generic;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using MyBlog.BL.Auth;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthentication authentication;

        public LoginController(IAuthentication authentication)
        {
            this.authentication = authentication;
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult Index(string u)
        {
            return View(new LoginViewModel()
            {
                U = u
            });
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> IndexPost(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isLocked = await authentication.IsAccountLocked(model.Email!);
                if (isLocked)
                {
                    ModelState.TryAddModelError("Email", "Аккаунт заблокирвован на 30 минут");
                    return View("Index", model);
                }

                var isAuthenticated = await authentication.Authenticate(model.Email!, model.Password!,
                        Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "", model.RememberMe);
                if (isAuthenticated)
                    return Redirect(String.IsNullOrEmpty(model.U) ? "/" : model.U);
                else
                    ModelState.TryAddModelError("Email", "Неверный Email или пароль");
            }
            return View("Index", model);
        }
    }
}

