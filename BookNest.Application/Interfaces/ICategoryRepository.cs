
using BookNest.Domain.Entities;

namespace BookNest.Application.Interfaces;

/// <summary>
/// Defines data access operations for book categories.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Gets a category by its identifier.
    /// </summary>
    Task<Category?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all categories.
    /// </summary>
    Task<IEnumerable<Category>> GetAllAsync();

    /// <summary>
    /// Adds a new category.
    /// </summary>
    Task AddAsync(Category category);

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    void Update(Category category);

    /// <summary>
    /// Deletes a category.
    /// </summary>
    void Delete(Category category);

    /// <summary>
    /// Checks whether a category name already exists.
    /// </summary>
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);

    /// <summary>
    /// Saves pending changes to the database.
    /// </summary>
    Task SaveChangesAsync();
}
