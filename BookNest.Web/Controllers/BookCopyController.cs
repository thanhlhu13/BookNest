
using BookNest.Application.DTOs.BookCopy;
using BookNest.Application.Interfaces;
using BookNest.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookNest.Web.Controllers;

/// <summary>
/// Handles book copy management requests.
/// </summary>
[Authorize(Roles = RoleConstants.Admin)]
public class BookCopyController : Controller
{
    private readonly IBookCopyService _bookCopyService;
    private readonly IBookService _bookService;

    public BookCopyController(
        IBookCopyService bookCopyService,
        IBookService bookService)
    {
        _bookCopyService = bookCopyService;
        _bookService = bookService;
    }

    public async Task<IActionResult> Index()
    {
        var bookCopies = await _bookCopyService.GetAllAsync();
        return View(bookCopies);
    }

    public async Task<IActionResult> Details(int id)
    {
        var bookCopy = await _bookCopyService.GetByIdAsync(id);

        if (bookCopy is null)
            return NotFound();

        return View(bookCopy);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadBooksAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookCopyDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadBooksAsync(dto.BookId);
            return View(dto);
        }

        var result = await _bookCopyService.CreateAsync(dto);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            await LoadBooksAsync(dto.BookId);
            return View(dto);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var bookCopy = await _bookCopyService.GetByIdAsync(id);

        if (bookCopy is null)
            return NotFound();

        var dto = new UpdateBookCopyDto
        {
            CopyCode = bookCopy.CopyCode,
            Status = bookCopy.Status
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateBookCopyDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _bookCopyService.UpdateAsync(id, dto);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(dto);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _bookCopyService.DeleteAsync(id);

        TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
            result.Message;

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Loads active books for the book selection dropdown.
    /// </summary>
    private async Task LoadBooksAsync(int? selectedBookId = null)
    {
        var books = await _bookService.GetAllAsync();

        ViewBag.Books = new SelectList(
            books.Where(b => b.IsActive),
            "Id",
            "Title",
            selectedBookId);
    }
}
