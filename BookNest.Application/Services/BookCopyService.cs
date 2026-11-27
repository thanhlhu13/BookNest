
using BookNest.Application.DTOs.BookCopy;
using BookNest.Application.Interfaces;
using BookNest.Domain.Entities;
using BookNest.Domain.Enums;

namespace BookNest.Application.Services;

/// <summary>
/// Implements business operations for book copies.
/// </summary>
public class BookCopyService : IBookCopyService
{
    private readonly IBookCopyRepository _bookCopyRepository;
    private readonly IBookRepository _bookRepository;

    public BookCopyService(
        IBookCopyRepository bookCopyRepository,
        IBookRepository bookRepository)
    {
        _bookCopyRepository = bookCopyRepository;
        _bookRepository = bookRepository;
    }

    public async Task<BookCopyDto?> GetByIdAsync(int id)
    {
        var bookCopy = await _bookCopyRepository.GetByIdAsync(id);

        return bookCopy is null ? null : MapToDto(bookCopy);
    }

    public async Task<IEnumerable<BookCopyDto>> GetAllAsync()
    {
        var bookCopies = await _bookCopyRepository.GetAllAsync();

        return bookCopies.Select(MapToDto);
    }

    public async Task<IEnumerable<BookCopyDto>> GetByBookIdAsync(int bookId)
    {
        var bookCopies = await _bookCopyRepository.GetByBookIdAsync(bookId);

        return bookCopies.Select(MapToDto);
    }

    public async Task<(bool Success, string Message)> CreateAsync(
        CreateBookCopyDto dto)
    {
        var copyCode = NormalizeText(dto.CopyCode);

        if (string.IsNullOrWhiteSpace(copyCode))
            return (false, "Copy code is required.");

        if (await _bookCopyRepository.ExistsByCopyCodeAsync(copyCode))
            return (false, "Copy code already exists.");

        var book = await _bookRepository.GetByIdAsync(dto.BookId);

        if (book is null)
            return (false, "Book not found.");

        if (!book.IsActive)
            return (false, "Cannot add copies to an inactive book.");

        var bookCopy = new BookCopy
        {
            BookId = dto.BookId,
            CopyCode = copyCode,
            Status = BookCopyStatus.Available,
            CreatedAt = DateTime.UtcNow
        };

        await _bookCopyRepository.AddAsync(bookCopy);
        await _bookCopyRepository.SaveChangesAsync();

        return (true, "Book copy created successfully.");
    }

    public async Task<(bool Success, string Message)> UpdateAsync(
        int id,
        UpdateBookCopyDto dto)
    {
        var bookCopy = await _bookCopyRepository.GetByIdAsync(id);

        if (bookCopy is null)
            return (false, "Book copy not found.");

        var copyCode = NormalizeText(dto.CopyCode);

        if (string.IsNullOrWhiteSpace(copyCode))
            return (false, "Copy code is required.");

        if (await _bookCopyRepository.ExistsByCopyCodeAsync(copyCode, id))
            return (false, "Copy code already exists.");

        if (!Enum.IsDefined(typeof(BookCopyStatus), dto.Status))
            return (false, "Invalid book copy status.");

        bookCopy.CopyCode = copyCode;
        bookCopy.Status = dto.Status;
        bookCopy.UpdatedAt = DateTime.UtcNow;

        _bookCopyRepository.Update(bookCopy);
        await _bookCopyRepository.SaveChangesAsync();

        return (true, "Book copy updated successfully.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var bookCopy = await _bookCopyRepository.GetByIdAsync(id);

        if (bookCopy is null)
            return (false, "Book copy not found.");

        _bookCopyRepository.Delete(bookCopy);
        await _bookCopyRepository.SaveChangesAsync();

        return (true, "Book copy deleted successfully.");
    }

    /// <summary>
    /// Maps a BookCopy entity to its DTO.
    /// </summary>
    private static BookCopyDto MapToDto(BookCopy bookCopy)
    {
        return new BookCopyDto
        {
            Id = bookCopy.Id,
            BookId = bookCopy.BookId,
            BookTitle = bookCopy.Book.Title,
            CopyCode = bookCopy.CopyCode,
            Status = bookCopy.Status,
            CreatedAt = bookCopy.CreatedAt,
            UpdatedAt = bookCopy.UpdatedAt
        };
    }

    /// <summary>
    /// Normalizes text by removing unnecessary whitespace.
    /// </summary>
    private static string NormalizeText(string? value)
    {
        return string.Join(
            " ",
            (value ?? string.Empty)
                .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
    }
}
