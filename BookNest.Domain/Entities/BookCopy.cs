
using BookNest.Domain.Enums;

namespace BookNest.Domain.Entities;

/// <summary>
/// Represents an individual physical copy of a book.
/// </summary>
public class BookCopy
{
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the book this physical copy belongs to.
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    /// Gets or sets the unique inventory code of this copy.
    /// </summary>
    public string CopyCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of this copy.
    /// </summary>
    public BookCopyStatus Status { get; set; } = BookCopyStatus.Available;

    /// <summary>
    /// Gets or sets the date when this copy was added to inventory.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last update date.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the related book.
    /// </summary>
    public Book Book { get; set; } = null!;
}
