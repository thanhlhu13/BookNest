
namespace BookNest.Application.DTOs.Book;

/// <summary>
/// Represents book information returned to the presentation layer.
/// </summary>
public class BookDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Publisher { get; set; }

    public DateOnly? PublishedDate { get; set; }

    public string? Language { get; set; }

    public string? CoverImageUrl { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
