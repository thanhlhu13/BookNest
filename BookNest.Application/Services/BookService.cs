
using BookNest.Application.DTOs.Book;
using BookNest.Application.Interfaces;
using BookNest.Domain.Entities;

namespace BookNest.Application.Services;

/// <summary>
/// Handles business operations for books.
/// </summary>
public class BookService : IBookService
{
    private readonly IBookRepository _repository;
    private readonly ICategoryRepository _categoryRepository;

    public BookService(
        IBookRepository repository,
        ICategoryRepository categoryRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
    }

    public async Task<BookDto?> GetByIdAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);

        return book is null ? null : MapToDto(book);
    }

    public async Task<IEnumerable<BookDto>> GetAllAsync()
    {
        var books = await _repository.GetAllAsync();

        return books.Select(MapToDto);
    }

    public async Task<(bool Success, string Message)> CreateAsync(
        CreateBookDto dto)
    {
        var title = NormalizeText(dto.Title);
        var isbn = NormalizeText(dto.ISBN);

        if (string.IsNullOrWhiteSpace(title))
            return (false, "Book title is required.");

        if (string.IsNullOrWhiteSpace(isbn))
            return (false, "ISBN is required.");

        if (await _repository.ExistsByISBNAsync(isbn))
            return (false, "ISBN already exists.");

        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);

        if (category is null || !category.IsActive)
            return (false, "Selected category is invalid or inactive.");

        var book = new Book
        {
            Title = title,
            ISBN = isbn,
            Description = NormalizeOptionalText(dto.Description),
            Publisher = NormalizeOptionalText(dto.Publisher),
            PublishedDate = dto.PublishedDate,
            Language = NormalizeOptionalText(dto.Language),
            CategoryId = dto.CategoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(book);
        await _repository.SaveChangesAsync();

        return (true, "Book created successfully.");
    }

    public async Task<(bool Success, string Message)> UpdateAsync(
        int id,
        UpdateBookDto dto)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book is null)
            return (false, "Book not found.");

        var title = NormalizeText(dto.Title);
        var isbn = NormalizeText(dto.ISBN);

        if (string.IsNullOrWhiteSpace(title))
            return (false, "Book title is required.");

        if (string.IsNullOrWhiteSpace(isbn))
            return (false, "ISBN is required.");

        if (await _repository.ExistsByISBNAsync(isbn, id))
            return (false, "ISBN already exists.");

        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);

        if (category is null || !category.IsActive)
            return (false, "Selected category is invalid or inactive.");

        book.Title = title;
        book.ISBN = isbn;
        book.Description = NormalizeOptionalText(dto.Description);
        book.Publisher = NormalizeOptionalText(dto.Publisher);
        book.PublishedDate = dto.PublishedDate;
        book.Language = NormalizeOptionalText(dto.Language);
        book.CategoryId = dto.CategoryId;
        book.IsActive = dto.IsActive;
        book.UpdatedAt = DateTime.UtcNow;

        _repository.Update(book);
        await _repository.SaveChangesAsync();

        return (true, "Book updated successfully.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book is null)
            return (false, "Book not found.");

        _repository.Delete(book);
        await _repository.SaveChangesAsync();

        return (true, "Book deleted successfully.");
    }

    private static BookDto MapToDto(Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            Description = book.Description,
            Publisher = book.Publisher,
            PublishedDate = book.PublishedDate,
            Language = book.Language,
            CoverImageUrl = book.CoverImageUrl,
            CategoryId = book.CategoryId,
            CategoryName = book.Category.Name,
            IsActive = book.IsActive,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt
        };
    }

    private static string NormalizeText(string? value)
    {
        return string.Join(
            " ",
            (value ?? string.Empty)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    private static string? NormalizeOptionalText(string? value)
    {
        var normalized = value?.Trim();

        return string.IsNullOrWhiteSpace(normalized)
            ? null
            : normalized;
    }
}
