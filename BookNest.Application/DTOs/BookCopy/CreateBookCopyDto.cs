
using System.ComponentModel.DataAnnotations;

namespace BookNest.Application.DTOs.BookCopy;

/// <summary>
/// Contains data required to create a physical book copy.
/// </summary>
public class CreateBookCopyDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int BookId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string CopyCode { get; set; } = string.Empty;
}
