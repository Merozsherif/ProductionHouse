using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Interfaces.ProductionHouse.Core.Interfaces;

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
        if (file == null)
            throw new Exception("Image is required.");

        if (file.Length == 0)
            throw new Exception("Image is empty.");

        var extension = Path.GetExtension(file.FileName).ToLower();

        string[] allowed =
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        if (!allowed.Contains(extension))
            throw new Exception("Invalid image type.");

        if (file.Length > 5 * 1024 * 1024)
            throw new Exception("Maximum image size is 5 MB.");

        var uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            folderName);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName =
            Guid.NewGuid().ToString() + extension;

        var filePath =
            Path.Combine(uploadsFolder, fileName);

        using var stream =
            new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);

        return $"uploads/{folderName}/{fileName}";
    }

    public void DeleteImage(string imagePath)
    {
        var fullPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            imagePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public async Task<List<string>> UploadManyAsync(
    List<IFormFile> files,
    string folderName)
    {
        var result = new List<string>();

        foreach (var file in files)
        {
            var path = await UploadAsync(file, folderName);

            result.Add(path);
        }

        return result;
    }

}