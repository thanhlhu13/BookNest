using BookNest.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Web.Controllers;

/// <summary>
/// Handles administrative pages and operations.
/// </summary>
[Authorize(Roles = RoleConstants.Admin)]
public class AdminController : Controller
{
    /// <summary>
    /// Displays the admin dashboard.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }
}