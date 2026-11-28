
using BookNest.Application.DTOs.Book;
using BookNest.Domain.Entities;

namespace BookNest.Application.Interfaces;

/// <summary>
/// Defines data access operations for books.
/// </summary>
public interface IBookRepository
{
    Task<Book?> GetByIdAsync(int id);

    Task<IEnumerable<Book>> GetAllAsync();

    Task AddAsync(Book book);

    void Update(Book book);

    void Delete(Book book);

    Task<bool> ExistsByISBNAsync(string isbn, int? excludeId = null);

    Task SaveChangesAsync();

    /// <summary>
    /// Retrieves books using search, filtering, sorting, and pagination.
    /// </summary>
    Task<(IEnumerable<Book> Items, int TotalCount)> GetPagedAsync(
        BookQueryDto query);
}
