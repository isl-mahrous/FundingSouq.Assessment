using FundingSouq.Assessment.Core.Extensions;
using FundingSouq.Assessment.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FundingSouq.Assessment.Application.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _environment;

    public FileUploadService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is not provided or is empty.");
        
        var givenFileName = file.FileName.IsNotEmpty() 
            ? Path.GetFileName(file.FileName).ToSnakeCase()
            : Guid.NewGuid().ToString();
        
        var fileName = $"{DateTimeOffset.Now.ToUnixTimeSeconds()}_{givenFileName}";
        var filePath = GetFilePath(fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }
    
    public async Task<string> UploadFileAsync(byte[] fileBytes, string fileName)
    {
        if (fileBytes == null || fileBytes.Length == 0)
            throw new ArgumentException("File data is not provided or is empty.");

        var givenFileName = fileName.IsNotEmpty() 
            ? Path.GetFileName(fileName).ToSnakeCase()
            : Guid.NewGuid().ToString();
        
        var finalFileName = $"{DateTimeOffset.Now.ToUnixTimeSeconds()}_{givenFileName}";
        var filePath = GetFilePath(finalFileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await stream.WriteAsync(fileBytes, 0, fileBytes.Length);

        return fileName;
    }

    private string GetFilePath(string fileName)
    {
        var uploadPath = Path.Combine(_environment.ContentRootPath, "files");

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        return Path.Combine(uploadPath, fileName);
    }
}