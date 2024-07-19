
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MyBlog.BL.Auth;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAuthentication authentication;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginController(IAuthentication authentication, IHttpContextAccessor httpContextAccessor)
        {
            this.authentication = authentication;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("/login")]
        public IActionResult Index(string u)
        {
            ViewBag.YaUrl = YaAuthViewModel.GetAuthUrl(httpContextAccessor);
            return View(new LoginViewModel()
            {
                U = u
            });
        }

        [HttpPost]        
        [Route("/login")]
        [EnableCors("Loginpolicytest")]
        public async Task<IActionResult> IndexPost(LoginViewModel model)
        {
            ViewBag.YaUrl = YaAuthViewModel.GetAuthUrl(httpContextAccessor);
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

