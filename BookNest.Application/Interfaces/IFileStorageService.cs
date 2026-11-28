namespace BookNest.Application.Interfaces;

/// <summary>
/// Defines file storage operations.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Uploads an image and returns its relative URL.
    /// </summary>
    Task<string> UploadImageAsync(
        Stream fileStream,
        string fileName,
        string contentType);

    /// <summary>
    /// Deletes an uploaded file using its relative URL.
    /// </summary>
    Task DeleteAsync(string fileUrl);
}