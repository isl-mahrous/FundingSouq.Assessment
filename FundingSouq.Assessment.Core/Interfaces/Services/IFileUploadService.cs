using Microsoft.AspNetCore.Http;

namespace FundingSouq.Assessment.Core.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<string> UploadFileAsync(byte[] fileBytes, string fileName);
}