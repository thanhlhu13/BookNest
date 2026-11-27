
using BookNest.Application.Interfaces;
using BookNest.Domain.Entities;
using BookNest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Infrastructure.Repositories;

/// <summary>
/// Implements data access operations for books.
/// </summary>
public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
    }

    public void Update(Book book)
    {
        _context.Books.Update(book);
    }

    public void Delete(Book book)
    {
        _context.Books.Remove(book);
    }

    public async Task<bool> ExistsByISBNAsync(
        string isbn,
        int? excludeId = null)
    {
        var normalizedIsbn = isbn.Trim();

        return await _context.Books.AnyAsync(b =>
            b.ISBN == normalizedIsbn &&
            (!excludeId.HasValue || b.Id != excludeId.Value));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
