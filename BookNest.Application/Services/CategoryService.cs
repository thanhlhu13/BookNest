
using BookNest.Application.DTOs.Category;
using BookNest.Application.Interfaces;
using BookNest.Domain.Entities;

namespace BookNest.Application.Services;

/// <summary>
/// Handles business operations for book categories.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);

        return category is null ? null : MapToDto(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();

        return categories.Select(MapToDto);
    }

    public async Task<(bool Success, string Message)> CreateAsync(
        CreateCategoryDto dto)
    {
        var name = NormalizeName(dto.Name);

        if (string.IsNullOrWhiteSpace(name))
            return (false, "Category name is required.");

        if (await _repository.ExistsByNameAsync(name))
            return (false, "Category name already exists.");

        var category = new Category
        {
            Name = name,
            Description = NormalizeDescription(dto.Description),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();

        return (true, "Category created successfully.");
    }

    public async Task<(bool Success, string Message)> UpdateAsync(
        int id,
        UpdateCategoryDto dto)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category is null)
            return (false, "Category not found.");

        var name = NormalizeName(dto.Name);

        if (string.IsNullOrWhiteSpace(name))
            return (false, "Category name is required.");

        if (await _repository.ExistsByNameAsync(name, id))
            return (false, "Category name already exists.");

        category.Name = name;
        category.Description = NormalizeDescription(dto.Description);
        category.IsActive = dto.IsActive;
        category.UpdatedAt = DateTime.UtcNow;

        _repository.Update(category);
        await _repository.SaveChangesAsync();

        return (true, "Category updated successfully.");
    }

    public async Task<(bool Success, string Message)> DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category is null)
            return (false, "Category not found.");

        _repository.Delete(category);
        await _repository.SaveChangesAsync();

        return (true, "Category deleted successfully.");
    }

    /// <summary>
    /// Converts a Category entity into a CategoryDto.
    /// </summary>
    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    /// <summary>
    /// Normalizes category names by trimming and collapsing whitespace.
    /// </summary>
    private static string NormalizeName(string? name)
    {
        return string.Join(
            " ",
            (name ?? string.Empty)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    /// <summary>
    /// Normalizes optional descriptions.
    /// </summary>
    private static string? NormalizeDescription(string? description)
    {
        var value = description?.Trim();

        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
