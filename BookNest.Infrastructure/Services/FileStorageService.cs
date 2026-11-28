using BookNest.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace BookNest.Infrastructure.Services;

/// <summary>
/// Handles local file storage operations.
/// </summary>
public class FileStorageService : IFileStorageService
{
    private readonly string _uploadDirectory;

    public FileStorageService(IHostEnvironment environment)
    {
        _uploadDirectory = Path.Combine(
            environment.ContentRootPath,
            "wwwroot",
            "images",
            "books");
    }

    /// <summary>
    /// Uploads an image and returns its relative URL.
    /// </summary>
    public async Task<string> UploadImageAsync(
        Stream fileStream,
        string fileName,
        string contentType)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        var newFileName = $"{Guid.NewGuid()}{extension}";

        Directory.CreateDirectory(_uploadDirectory);

        var filePath = Path.Combine(_uploadDirectory, newFileName);

        await using var outputStream = new FileStream(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write);

        await fileStream.CopyToAsync(outputStream);

        return $"/images/books/{newFileName}";
    }

    /// <summary>
    /// Deletes an uploaded file using its relative URL.
    /// </summary>
    public Task DeleteAsync(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            return Task.CompletedTask;

        var fileName = Path.GetFileName(fileUrl);
        var filePath = Path.Combine(_uploadDirectory, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}