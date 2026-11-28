
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
    private readonly IFileStorageService _fileStorageService;

    public BookController(
        IBookService bookService,
        ICategoryService categoryService,
        IFileStorageService fileStorageService)
    {
        _bookService = bookService;
        _categoryService = categoryService;
        _fileStorageService = fileStorageService;
    }

    /// <summary>
    /// Displays books with search, filtering, sorting, and pagination.
    /// </summary>
    public async Task<IActionResult> Index(BookQueryDto query)
    {
        var result = await _bookService.GetPagedAsync(query);

        await LoadCategoriesAsync(query.CategoryId);

        ViewBag.Query = query;

        return View(result);
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

    /// <summary>
    /// Creates a new book with an optional cover image.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateBookDto dto,
        IFormFile? CoverImage)
    {
        // Validate uploaded cover image.
        if (CoverImage is not null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var fileExtension = Path.GetExtension(CoverImage.FileName)
                .ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError(
                    "CoverImage",
                    "Only JPG, JPEG, PNG, and WEBP images are allowed.");
            }

            if (CoverImage.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError(
                    "CoverImage",
                    "Image size must not exceed 5 MB.");
            }

            if (CoverImage.Length == 0)
            {
                ModelState.AddModelError(
                    "CoverImage",
                    "The uploaded image is empty.");
            }
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(dto.CategoryId);
            return View(dto);
        }

        // Upload the cover image if provided.
        if (CoverImage is not null)
        {
            await using var stream = CoverImage.OpenReadStream();

            dto.CoverImageUrl = await _fileStorageService.UploadImageAsync(
                stream,
                CoverImage.FileName,
                CoverImage.ContentType);
        }

        var result = await _bookService.CreateAsync(dto);

        if (!result.Success)
        {
            // Delete the uploaded image if book creation fails.
            if (!string.IsNullOrWhiteSpace(dto.CoverImageUrl))
            {
                await _fileStorageService.DeleteAsync(dto.CoverImageUrl);
            }

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
        ViewBag.CoverImageUrl = book.CoverImageUrl;
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
    int id,
    UpdateBookDto dto,
    IFormFile? CoverImage)
    {
        // Validate uploaded cover image.
        if (CoverImage is not null)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var fileExtension = Path.GetExtension(CoverImage.FileName)
                .ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError(
                    nameof(CoverImage),
                    "Only JPG, JPEG, PNG, and WEBP images are allowed.");
            }

            if (CoverImage.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError(
                    nameof(CoverImage),
                    "Image size must not exceed 5 MB.");
            }

            if (CoverImage.Length == 0)
            {
                ModelState.AddModelError(
                    nameof(CoverImage),
                    "The uploaded image is empty.");
            }
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(dto.CategoryId);
            return View(dto);
        }

        // Get the current book and preserve its existing cover image URL.
        var existingBook = await _bookService.GetByIdAsync(id);

        if (existingBook is null)
        {
            return NotFound();
        }

        var oldCoverImageUrl = existingBook.CoverImageUrl;

        // Upload the new cover image if provided.
        if (CoverImage is not null)
        {
            await using var stream = CoverImage.OpenReadStream();

            dto.CoverImageUrl = await _fileStorageService.UploadImageAsync(
                stream,
                CoverImage.FileName,
                CoverImage.ContentType);
        }

        var result = await _bookService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            // Delete the newly uploaded image if book update fails.
            if (CoverImage is not null &&
                !string.IsNullOrWhiteSpace(dto.CoverImageUrl))
            {
                await _fileStorageService.DeleteAsync(dto.CoverImageUrl);
            }

            ModelState.AddModelError(string.Empty, result.Message);
            await LoadCategoriesAsync(dto.CategoryId);
            ViewBag.CoverImageUrl = oldCoverImageUrl;
            return View(dto);
        }

        // Delete the old cover image after successful update.
        if (CoverImage is not null &&
            !string.IsNullOrWhiteSpace(oldCoverImageUrl))
        {
            await _fileStorageService.DeleteAsync(oldCoverImageUrl);
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
