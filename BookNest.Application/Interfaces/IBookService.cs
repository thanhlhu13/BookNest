
using BookNest.Application.DTOs.Book;
using BookNest.Application.DTOs.Common;

namespace BookNest.Application.Interfaces;

/// <summary>
/// Defines business operations for books.
/// </summary>
public interface IBookService
{
    Task<BookDto?> GetByIdAsync(int id);

    Task<IEnumerable<BookDto>> GetAllAsync();

    Task<(bool Success, string Message)> CreateAsync(CreateBookDto dto);

    Task<(bool Success, string Message)> UpdateAsync(int id, UpdateBookDto dto);

    Task<(bool Success, string Message)> DeleteAsync(int id);

    /// <summary>
    /// Retrieves books with search, filtering, sorting, and pagination.
    /// </summary>
    Task<PagedResultDto<BookDto>> GetPagedAsync(BookQueryDto query);
}
