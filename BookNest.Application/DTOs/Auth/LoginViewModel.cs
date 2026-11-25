using System.ComponentModel.DataAnnotations;

namespace BookNest.Application.DTOs.Auth;

/// <summary>
/// Represents the data required to authenticate a user.
/// </summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "Email or username is required.")]
    public string EmailOrUserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}