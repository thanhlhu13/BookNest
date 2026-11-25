using BookNest.Application.Interfaces;
using BookNest.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookNest.Application;

/// <summary>
/// Registers application layer services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}