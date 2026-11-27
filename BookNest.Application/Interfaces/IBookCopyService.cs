
using BookNest.Application.DTOs.BookCopy;

namespace BookNest.Application.Interfaces;

/// <summary>
/// Defines business operations for book copies.
/// </summary>
public interface IBookCopyService
{
    Task<BookCopyDto?> GetByIdAsync(int id);

    Task<IEnumerable<BookCopyDto>> GetAllAsync();

    Task<IEnumerable<BookCopyDto>> GetByBookIdAsync(int bookId);

    Task<(bool Success, string Message)> CreateAsync(
        CreateBookCopyDto dto);

    Task<(bool Success, string Message)> UpdateAsync(
        int id,
        UpdateBookCopyDto dto);

    Task<(bool Success, string Message)> DeleteAsync(int id);
}
