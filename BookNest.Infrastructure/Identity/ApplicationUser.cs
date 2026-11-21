
using Microsoft.AspNetCore.Identity;

namespace BookNest.Infrastructure.Identity;

/// <summary>
/// Represents an application user in the BookNest system.
/// Extends ASP.NET Core Identity with additional profile information.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Gets or sets the user's full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's address.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets the account creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Indicates whether the account is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
