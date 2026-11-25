
using BookNest.Application.Interfaces;
using BookNest.Domain.Entities;
using BookNest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Infrastructure.Repositories;

/// <summary>
/// Implements data access operations for book categories.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }

    public async Task<bool> ExistsByNameAsync(
        string name,
        int? excludeId = null)
    {
        var normalizedName = name.Trim();

        return await _context.Categories.AnyAsync(c =>
            c.Name == normalizedName &&
            (!excludeId.HasValue || c.Id != excludeId.Value));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
