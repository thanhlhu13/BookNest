
namespace BookNest.Application.DTOs.Book;

/// <summary>
/// Contains search, filter, sorting, and pagination parameters for books.
/// </summary>
public class BookQueryDto
{
    /// <summary>
    /// Gets or sets the search keyword for book title or ISBN.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Gets or sets the category filter.
    /// </summary>
    public int? CategoryId { get; set; }

    /// <summary>
    /// Gets or sets the active status filter.
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Gets or sets the field used for sorting.
    /// </summary>
    public string SortBy { get; set; } = "Title";

    /// <summary>
    /// Gets or sets the sorting direction.
    /// </summary>
    public string SortDirection { get; set; } = "asc";

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 10;
}
