
namespace BookNest.Domain.Entities;

/// <summary>
/// Represents a book title in the catalog.
/// </summary>
public class Book
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

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public Category Category { get; set; } = null!;
}
