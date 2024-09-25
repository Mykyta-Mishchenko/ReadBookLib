using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReadBookLib.Data;
using ReadBookLib.Interfaces;
using ReadBookLib.Models;
using ReadBookLib.Repository;
using ReadBookLib.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataDbContext>(opts =>
{
	opts.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataDbContext>().AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFileService, LocalFileService>();
builder.Services.AddScoped<IBookRepository, DbBookRepository>();

builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.Password.RequiredLength = 8;
    opts.Password.RequireDigit = true;

    opts.Lockout.MaxFailedAccessAttempts = 3;
    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

    opts.User.RequireUniqueEmail = true;
    opts.SignIn.RequireConfirmedEmail = false;
});

builder.Services.ConfigureApplicationCookie(opts =>
{
	opts.LoginPath = "/Identity/SignIn";
	opts.AccessDeniedPath = "/Identity/AccessDenied";
	opts.ExpireTimeSpan = TimeSpan.FromHours(1);
});

builder.Services.AddAuthentication()
    .AddGoogle(opts =>
    {
        opts.ClientId = builder.Configuration["GoogleAppId"];
        opts.ClientSecret = builder.Configuration["GoogleAppSecret"];
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
}

app.MapDefaultControllerRoute();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");
app.MapControllerRoute(
    name: "managementDefault",
    pattern: "{controller=Management}/{action=Index}");


app.MapRazorPages();

app.UseMiddleware<FileExceptionMiddleware>();

app.Run();
