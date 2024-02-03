using MyBlog.BL.Auth;
using MyBlog.BL.Email;
using MyBlog.BL.Blog;
using MyBlog.DAL.Implementations;
using MyBlog.DAL.Implementations.Ado;
using MyBlog.DAL.Implementations.Dapper;
using MyBlog.DAL.Implementations.EF;
using MyBlog.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// для простоты использую только синглтон
builder.Services.AddSingleton<IEmailQueue, EmailQueue>();
builder.Services.AddSingleton<IAuthentication, Authentication>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddSingleton<IUser, User>();
builder.Services.AddSingleton<IEncrypt, Pbkdf2Encrypt>();
builder.Services.AddSingleton<IFailedAttemptDAL, FailedAttemptDAL>();
builder.Services.AddSingleton<IUserSecurity, UserSecurity>();
builder.Services.AddSingleton<IBlog, Blog>();
builder.Services.AddSingleton<MyBlog.BL.Auth.ISession, MyBlog.BL.Auth.Session>();

builder.Services.AddSingleton<ISessionDAL, SessionDAL>();
builder.Services.AddSingleton<IEmailQueueDAL, EmailQueueDAL>();
builder.Services.AddSingleton<IAuthenticationDAL, AdoAuthenticationDAL>();
builder.Services.AddSingleton<IUserSecurityDAL, UserSecurityDAL>();
builder.Services.AddSingleton<IBlogDAL, BlogDAL>();


builder.Services.AddResponseCaching();
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default",
        new CacheProfile() { Duration = 100 });
});


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<ICaptcha, DevCaptcha>();
    builder.Services.AddSingleton<IEmailClient, DevEmailClient>();
}
else
{
    builder.Services.AddSingleton<ICaptcha>(x => new GoogleCaptcha(builder.Configuration["Captcha:SiteKey"] ?? "", builder.Configuration["Captcha:SecretKey"]??""));
    builder.Services.AddSingleton<IEmailClient>(x => new EmailClient(builder.Configuration["EmailClient:server"] ?? "",builder.Configuration["EmailClient:username"]??"", builder.Configuration["EmailClient:password"]?? ""));
}


builder.Services.AddMvc()
        .AddSessionStateTempDataProvider();

builder.Services.AddHttpClient();
builder.Services.AddSession();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Session.GetString("userid") ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));
});


builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter("fixed",
    rateLimiter => {
        rateLimiter.Window = TimeSpan.FromSeconds(12);
        rateLimiter.PermitLimit = 4;
        rateLimiter.QueueLimit = 2;
        rateLimiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    })
);

builder.Services.AddRateLimiter(_ => _.AddSlidingWindowLimiter("sliding",
    rateLimiter => {
        rateLimiter.Window = TimeSpan.FromSeconds(12);
        rateLimiter.PermitLimit = 4;
        rateLimiter.SegmentsPerWindow = 4;
    })
);

builder.Services.AddDataProtection();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.UseRateLimiter();
app.MapDefaultControllerRoute();

app.Run();

