
using BookNest.Domain.Enums;

namespace BookNest.Application.DTOs.BookCopy;

/// <summary>
/// Represents a book copy returned to the presentation layer.
/// </summary>
public class BookCopyDto
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string BookTitle { get; set; } = string.Empty;

    public string CopyCode { get; set; } = string.Empty;

    public BookCopyStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
