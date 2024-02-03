using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyBlog.Extensions
{
    public static class MyBlogExt
    {
        public static IHtmlContent FormantContent(this IHtmlHelper htmlHelper, string str)
        {
            return new HtmlString(str
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\n", "</p><p>"));
        }
    }
}
