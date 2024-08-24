using Microsoft.AspNetCore.Http;

namespace FundingSouq.Assessment.Core.Interfaces;

/// <summary>
/// Defines methods for uploading files.
/// </summary>
public interface IFileUploadService
{
    /// <summary>
    /// Uploads a file from an <see cref="IFormFile"/> and returns the saved file name.
    /// </summary>
    /// <param name="file">The file to upload.</param>
    /// <returns>The name of the saved file.</returns>
    Task<string> UploadFileAsync(IFormFile file);

    /// <summary>
    /// Uploads a file from a byte array and returns the saved file name.
    /// </summary>
    /// <param name="fileBytes">The file data as a byte array.</param>
    /// <param name="fileName">The original file name.</param>
    /// <returns>The name of the saved file.</returns>
    Task<string> UploadFileAsync(byte[] fileBytes, string fileName);
}