
using BookNest.Domain.Entities;

namespace BookNest.Application.Interfaces;

/// <summary>
/// Defines data access operations for book copies.
/// </summary>
public interface IBookCopyRepository
{
    Task<BookCopy?> GetByIdAsync(int id);

    Task<IEnumerable<BookCopy>> GetAllAsync();

    Task<IEnumerable<BookCopy>> GetByBookIdAsync(int bookId);

    Task AddAsync(BookCopy bookCopy);

    void Update(BookCopy bookCopy);

    void Delete(BookCopy bookCopy);

    Task<bool> ExistsByCopyCodeAsync(
        string copyCode,
        int? excludeId = null);

    Task SaveChangesAsync();
}
