
using System.ComponentModel.DataAnnotations;

namespace BookNest.Application.DTOs.Category;

/// <summary>
/// Represents the data required to update an existing category.
/// </summary>
public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;
}
