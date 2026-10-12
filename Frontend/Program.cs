using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ServerCSharp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add Database with SQLite (dùng chung database.db tại Backend)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=../Backend/database.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Add MVC Controllers & Razor Views
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    });

// Add Cookie Authentication for Web Session & Role switching
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

builder.Services.AddAuthorization();

// HttpClient to communicate with Backend Web API
var backendUrl = builder.Configuration["BackendApi:BaseUrl"] ?? "http://localhost:5000";
builder.Services.AddHttpClient("BackendApi", client =>
{
    client.BaseAddress = new Uri(backendUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

// Enable Static Files (css/site.css, images, js)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map MVC Default Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
