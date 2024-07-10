namespace MyBlog.Middleware;

public class XssTestMiddleware
{
    private readonly RequestDelegate next;

    public XssTestMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task  Invoke(HttpContext context)
    {

        await this.next(context);

        string paramvalue = context.Request.Query["xssmiddletest"].FirstOrDefault() ?? "";
        await context.Response.WriteAsync(
            $@"
            <div class='dev-performance'>
                {paramvalue}
            </div>
            "
            );
    }
}
