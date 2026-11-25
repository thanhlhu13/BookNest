
using BookNest.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BookNest.Infrastructure.Identity;

/// <summary>
/// Seeds default application roles into the database.
/// </summary>
public static class RoleSeeder
{
    /// <summary>
    /// Creates default roles if they do not already exist.
    /// </summary>
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles =
        {
            RoleConstants.Admin,
            RoleConstants.Customer
        };

        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
