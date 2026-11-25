using BookNest.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Web.Controllers;

/// <summary>
/// Handles customer-specific pages and operations.
/// </summary>
[Authorize(Roles = RoleConstants.Customer)]
public class CustomerController : Controller
{
    /// <summary>
    /// Displays the customer dashboard.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }
}