
using BookNest.Application.DTOs.Auth;
using BookNest.Domain.Constants;
using BookNest.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Web.Controllers;

/// <summary>
/// Handles user account operations such as registration, login, and logout.
/// </summary>
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Displays the user registration page.
    /// </summary>
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    /// <summary>
    /// Displays the user login page.
    /// </summary>
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    /// <summary>
    /// Displays the access denied page when a user lacks permission.
    /// </summary>
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }


    /// <summary>
    /// Processes a new customer registration request.
    /// </summary>

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check whether the email already exists.
        var existingEmail = await _userManager.FindByEmailAsync(model.Email);

        if (existingEmail != null)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "This email is already registered.");

            return View(model);
        }

        // Check whether the username already exists.
        var existingUserName = await _userManager.FindByNameAsync(model.UserName);

        if (existingUserName != null)
        {
            ModelState.AddModelError(
                nameof(model.UserName),
                "This username is already taken.");

            return View(model);
        }

        // Create a new user entity.
        var user = new ApplicationUser
        {
            FullName = model.FullName,
            Email = model.Email,
            UserName = model.UserName,
            Address = model.Address,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Create the user account with the provided password.
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // Assign the Customer role after successful user creation.
        var roleResult = await _userManager.AddToRoleAsync(
            user,
            RoleConstants.Customer);

        if (!roleResult.Succeeded)
        {
            foreach (var error in roleResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            // Remove the newly created user if role assignment fails.
            await _userManager.DeleteAsync(user);

            return View(model);
        }

        TempData["SuccessMessage"] =
            "Registration successful. You can now log in.";

        return RedirectToAction(nameof(Login));
    }


    /// <summary>
    /// Authenticates a user using email or username and password.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Find the user by email or username.
        var user = await _userManager.FindByEmailAsync(model.EmailOrUserName);

        if (user == null)
        {
            user = await _userManager.FindByNameAsync(model.EmailOrUserName);
        }

        if (user == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "Invalid email/username or password.");

            return View(model);
        }


        // Check whether the account is active.
        if (!user.IsActive)
        {
            ModelState.AddModelError(
                string.Empty,
                "Your account has been disabled. Please contact support.");

            return View(model);
        }

        // Verify the password and sign in the user.
        var result = await _signInManager.PasswordSignInAsync(
            user,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Login successful. Welcome back!";

            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(
                string.Empty,
                "Your account has been locked due to multiple failed login attempts.");

            return View(model);
        }

        ModelState.AddModelError(
            string.Empty,
            "Invalid email/username or password.");

        return View(model);
    }

    /// <summary>
    /// Signs out the currently authenticated user.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        TempData["SuccessMessage"] = "You have been logged out successfully.";

        return RedirectToAction("Index", "Home");
    }


}
