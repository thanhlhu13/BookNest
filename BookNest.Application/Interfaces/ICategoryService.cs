
using BookNest.Application.DTOs.Category;

namespace BookNest.Application.Interfaces;

/// <summary>
/// Defines business operations for book categories.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Gets a category by its identifier.
    /// </summary>
    Task<CategoryDto?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all categories.
    /// </summary>
    Task<IEnumerable<CategoryDto>> GetAllAsync();

    /// <summary>
    /// Creates a new category.
    /// </summary>
    Task<(bool Success, string Message)> CreateAsync(
        CreateCategoryDto dto);

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    Task<(bool Success, string Message)> UpdateAsync(
        int id,
        UpdateCategoryDto dto);

    /// <summary>
    /// Deletes a category.
    /// </summary>
    Task<(bool Success, string Message)> DeleteAsync(int id);
}
