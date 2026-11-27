
using BookNest.Application.Interfaces;
using BookNest.Domain.Entities;
using BookNest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Infrastructure.Repositories;

/// <summary>
/// Implements data access operations for book copies.
/// </summary>
public class BookCopyRepository : IBookCopyRepository
{
    private readonly ApplicationDbContext _context;

    public BookCopyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BookCopy?> GetByIdAsync(int id)
    {
        return await _context.BookCopies
            .Include(bc => bc.Book)
            .FirstOrDefaultAsync(bc => bc.Id == id);
    }

    public async Task<IEnumerable<BookCopy>> GetAllAsync()
    {
        return await _context.BookCopies
            .AsNoTracking()
            .Include(bc => bc.Book)
            .OrderBy(bc => bc.Book.Title)
            .ThenBy(bc => bc.CopyCode)
            .ToListAsync();
    }

    public async Task<IEnumerable<BookCopy>> GetByBookIdAsync(int bookId)
    {
        return await _context.BookCopies
            .AsNoTracking()
            .Include(bc => bc.Book)
            .Where(bc => bc.BookId == bookId)
            .OrderBy(bc => bc.CopyCode)
            .ToListAsync();
    }

    public async Task AddAsync(BookCopy bookCopy)
    {
        await _context.BookCopies.AddAsync(bookCopy);
    }

    public void Update(BookCopy bookCopy)
    {
        _context.BookCopies.Update(bookCopy);
    }

    public void Delete(BookCopy bookCopy)
    {
        _context.BookCopies.Remove(bookCopy);
    }

    public async Task<bool> ExistsByCopyCodeAsync(
        string copyCode,
        int? excludeId = null)
    {
        var normalizedCode = copyCode.Trim();

        return await _context.BookCopies.AnyAsync(bc =>
            bc.CopyCode == normalizedCode &&
            (!excludeId.HasValue || bc.Id != excludeId.Value));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
