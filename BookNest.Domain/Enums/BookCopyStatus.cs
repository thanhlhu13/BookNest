
namespace BookNest.Domain.Enums;

/// <summary>
/// Defines the availability and condition statuses of a book copy.
/// </summary>
public enum BookCopyStatus
{
    Available = 1,
    Rented = 2,
    Reserved = 3,
    Damaged = 4,
    Lost = 5
}
