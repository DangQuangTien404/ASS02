using DAL.Contexts;
using DAL.Repositories;
using DangQuangTien_RazorPages;
using DangQuangTien_RazorPages.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.DTOs;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

// 2) Configs
builder.Services.Configure<AdminAccountSettings>(
    builder.Configuration.GetSection("AdminAccount"));

// 3) Authentication + Authorization
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });
builder.Services.AddAuthorization();

// 4) Repositories
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();

// 5) Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsService, NewsService>();

// 6) Razor, Session, SignalR
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSignalR();
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddRazorPages();

var app = builder.Build();

// 7) SIGNALR DELEGATE BINDING
using (var scope = app.Services.CreateScope())
{
    var newsSvc = scope.ServiceProvider.GetRequiredService<INewsService>();
    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NewsHub>>();

    newsSvc.OnArticlePublished += article =>
    {
        return hubContext.Clients.All.SendAsync("ReceiveNewArticle",
            article.NewsTitle,
            $"/News/Details/{article.NewsArticleId}");
    };
}

// 8) PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapHub<NewsHub>("/newsHub");
app.Run();
