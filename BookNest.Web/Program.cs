
using BookNest.Infrastructure;
using BookNest.Infrastructure.Identity;
using BookNest.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using BookNest.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register Application services.
builder.Services.AddApplication();

// Register Infrastructure services.
builder.Services.AddInfrastructure(builder.Configuration);

// Register ASP.NET Core Identity.

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Password configuration.
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;

        // User configuration.
        options.User.RequireUniqueEmail = true;

        // Account lockout configuration.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cookie authentication configuration.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "BookNest.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});


var app = builder.Build();

// Seed default application roles.
await RoleSeeder.SeedRolesAsync(app.Services);

// Seed initial administrator account.
await AdminSeeder.SeedAdminAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authenticate the current user.
app.UseAuthentication();

// Authorize the current user.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
