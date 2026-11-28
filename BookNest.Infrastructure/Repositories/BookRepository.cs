
using BookNest.Application.DTOs.Book;
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


    /// <summary>
    /// Retrieves books using search, filtering, sorting, and pagination.
    /// </summary>
    public async Task<(IEnumerable<Book> Items, int TotalCount)> GetPagedAsync(
        BookQueryDto query)
    {
        var booksQuery = _context.Books
            .AsNoTracking()
            .Include(b => b.Category)
            .AsQueryable();

        // Search by book title or ISBN.
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim();

            booksQuery = booksQuery.Where(b =>
                b.Title.Contains(searchTerm) ||
                b.ISBN.Contains(searchTerm));
        }

        // Filter by category.
        if (query.CategoryId.HasValue)
        {
            booksQuery = booksQuery.Where(b =>
                b.CategoryId == query.CategoryId.Value);
        }

        // Filter by active status.
        if (query.IsActive.HasValue)
        {
            booksQuery = booksQuery.Where(b =>
                b.IsActive == query.IsActive.Value);
        }

        // Count records before pagination.
        var totalCount = await booksQuery.CountAsync();

        // Sorting.
        booksQuery = (query.SortBy.ToLower(), query.SortDirection.ToLower()) switch
        {
            ("title", "desc") => booksQuery.OrderByDescending(b => b.Title),
            ("isbn", "asc") => booksQuery.OrderBy(b => b.ISBN),
            ("isbn", "desc") => booksQuery.OrderByDescending(b => b.ISBN),
            ("publisher", "asc") => booksQuery.OrderBy(b => b.Publisher),
            ("publisher", "desc") => booksQuery.OrderByDescending(b => b.Publisher),
            ("createdat", "asc") => booksQuery.OrderBy(b => b.CreatedAt),
            ("createdat", "desc") => booksQuery.OrderByDescending(b => b.CreatedAt),
            ("publisheddate", "asc") => booksQuery.OrderBy(b => b.PublishedDate),
            ("publisheddate", "desc") => booksQuery.OrderByDescending(b => b.PublishedDate),
            _ => booksQuery.OrderBy(b => b.Title)
        };

        // Pagination.
        var pageNumber = Math.Max(1, query.PageNumber);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);

        var items = await booksQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

}
