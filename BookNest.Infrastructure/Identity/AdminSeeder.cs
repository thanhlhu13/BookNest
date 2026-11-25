using BookNest.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookNest.Infrastructure.Identity;

/// <summary>
/// Seeds the initial administrator account.
/// </summary>
public static class AdminSeeder
{
    /// <summary>
    /// Creates the initial administrator account if it does not exist.
    /// </summary>
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        var adminEmail = "admin@booknest.com";
        var adminUserName = "admin";
        var adminPassword = "Admin@12345";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                FullName = "BookNest Administrator",
                Email = adminEmail,
                UserName = adminUserName,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await userManager.CreateAsync(admin, adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create admin account: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, RoleConstants.Admin))
        {
            var roleResult = await userManager.AddToRoleAsync(
                admin,
                RoleConstants.Admin);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Failed to assign Admin role: {errors}");
            }
        }
    }
}