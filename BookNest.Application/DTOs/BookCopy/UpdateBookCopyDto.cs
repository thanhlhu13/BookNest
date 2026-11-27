
using System.ComponentModel.DataAnnotations;
using BookNest.Domain.Enums;

namespace BookNest.Application.DTOs.BookCopy;

/// <summary>
/// Contains data required to update a physical book copy.
/// </summary>
public class UpdateBookCopyDto
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string CopyCode { get; set; } = string.Empty;

    [Required]
    public BookCopyStatus Status { get; set; }
}
