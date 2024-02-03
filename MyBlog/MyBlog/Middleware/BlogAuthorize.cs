using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyBlog.Middleware
{
    public enum AuthRole { User, Admin }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class BlogAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private AuthRole role;

        public BlogAuthorizeAttribute(AuthRole role)
        {
            this.role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext?.Session.GetInt32("userid") == null)
            {
                context.Result = new RedirectResult("/Login");
                return;
            }

            if (context.HttpContext?.Session.GetString("Role") != role.ToString())
            {
//                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            }
        }
    }
}