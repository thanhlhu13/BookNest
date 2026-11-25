
using BookNest.Application.DTOs.Category;
using BookNest.Application.Interfaces;
using BookNest.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookNest.Web.Controllers;

/// <summary>
/// Handles category management requests.
/// </summary>
[Authorize(Roles = RoleConstants.Admin)]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Displays all categories.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();

        return View(categories);
    }

    /// <summary>
    /// Displays category details.
    /// </summary>
    public async Task<IActionResult> Details(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category is null)
            return NotFound();

        return View(category);
    }

    /// <summary>
    /// Displays the category creation form.
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _categoryService.CreateAsync(dto);

        if (!result.Success)
        {
            ModelState.AddModelError(nameof(dto.Name), result.Message);
            return View(dto);
        }

        TempData["SuccessMessage"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Displays the category editing form.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);

        if (category is null)
            return NotFound();

        var dto = new UpdateCategoryDto
        {
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };

        return View(dto);
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _categoryService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            ModelState.AddModelError(nameof(dto.Name), result.Message);
            return View(dto);
        }

        TempData["SuccessMessage"] = result.Message;

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Deletes a category.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteAsync(id);

        TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
            result.Message;

        return RedirectToAction(nameof(Index));
    }
}
