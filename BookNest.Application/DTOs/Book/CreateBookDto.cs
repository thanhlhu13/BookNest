
using System.ComponentModel.DataAnnotations;

namespace BookNest.Application.DTOs.Book;

/// <summary>
/// Represents the data required to create a new book.
/// </summary>
public class CreateBookDto
{
    [Required(ErrorMessage = "Book title is required.")]
    [StringLength(200, ErrorMessage = "Book title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "ISBN is required.")]
    [StringLength(20, ErrorMessage = "ISBN cannot exceed 20 characters.")]
    public string ISBN { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    public string? Description { get; set; }

    [StringLength(200, ErrorMessage = "Publisher cannot exceed 200 characters.")]
    public string? Publisher { get; set; }

    public DateOnly? PublishedDate { get; set; }

    [StringLength(50, ErrorMessage = "Language cannot exceed 50 characters.")]
    public string? Language { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category.")]
    public int CategoryId { get; set; }
}
