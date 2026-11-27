
using BookNest.Application.DTOs.Book;
using BookNest.Application.Interfaces;
using BookNest.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookNest.Web.Controllers;

/// <summary>
/// Handles book management requests.
/// </summary>
[Authorize(Roles = RoleConstants.Admin)]
public class BookController : Controller
{
    private readonly IBookService _bookService;
    private readonly ICategoryService _categoryService;

    public BookController(
        IBookService bookService,
        ICategoryService categoryService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var books = await _bookService.GetAllAsync();
        return View(books);
    }

    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book is null)
            return NotFound();

        return View(book);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(dto.CategoryId);
            return View(dto);
        }

        var result = await _bookService.CreateAsync(dto);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            await LoadCategoriesAsync(dto.CategoryId);
            return View(dto);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookService.GetByIdAsync(id);

        if (book is null)
            return NotFound();

        var dto = new UpdateBookDto
        {
            Title = book.Title,
            ISBN = book.ISBN,
            Description = book.Description,
            Publisher = book.Publisher,
            PublishedDate = book.PublishedDate,
            Language = book.Language,
            CategoryId = book.CategoryId,
            IsActive = book.IsActive
        };

        await LoadCategoriesAsync(dto.CategoryId);
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateBookDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(dto.CategoryId);
            return View(dto);
        }

        var result = await _bookService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            await LoadCategoriesAsync(dto.CategoryId);
            return View(dto);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookService.DeleteAsync(id);

        TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
            result.Message;

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Loads active categories into the category dropdown.
    /// </summary>
    private async Task LoadCategoriesAsync(int? selectedCategoryId = null)
    {
        var categories = await _categoryService.GetAllAsync();

        ViewBag.Categories = new SelectList(
            categories.Where(c => c.IsActive),
            "Id",
            "Name",
            selectedCategoryId);
    }
}
