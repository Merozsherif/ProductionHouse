
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using ProductionHouse.Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
namespace ProductionHouse.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadAsync(IFormFile file, string folderName)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Image is required.");

        var extension = Path.GetExtension(file.FileName).ToLower();

        var allowedExtensions = new[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        if (!allowedExtensions.Contains(extension))
            throw new Exception("Invalid image format.");

        if (file.Length > 5 * 1024 * 1024)
            throw new Exception("Image size cannot exceed 5 MB.");

        var uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            folderName);

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(uploadsFolder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);

        return $"uploads/{folderName}/{fileName}";
    }
}