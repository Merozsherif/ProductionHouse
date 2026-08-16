using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProductionHouse.Core.Interfaces;


namespace ProductionHouse.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    // =====================================================
    // UPLOAD IMAGE -> WEBP
    // =====================================================
    // في ImageService.cs
    public void MoveImage(string oldRelativePath, string newRelativePath)
    {
        if (string.IsNullOrWhiteSpace(oldRelativePath))
            return;

        var oldFullPath = Path.Combine(
            _environment.WebRootPath,
            oldRelativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

        var newFullPath = Path.Combine(
            _environment.WebRootPath,
            newRelativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

        if (!File.Exists(oldFullPath))
            return; // الملف مش موجود أصلاً، تجاهله

        var newDir = Path.GetDirectoryName(newFullPath);
        if (!string.IsNullOrEmpty(newDir))
            Directory.CreateDirectory(newDir);

        if (oldFullPath == newFullPath)
            return; // نفس المكان أصلاً، متعملش حاجة

        File.Move(oldFullPath, newFullPath, overwrite: true);
    }
    public async Task<string> UploadAsync(
        IFormFile file,
        string folderName,
        string fileName = "image")
    {
        if (file == null)
            throw new Exception("Image is required.");

        if (file.Length == 0)
            throw new Exception("Image is empty.");

        if (file.Length > 15 * 1024 * 1024)
            throw new Exception("Maximum image size is 15 MB.");

        var extension = Path.GetExtension(file.FileName)
            .ToLowerInvariant();

        var allowedExtensions = new[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp",
            ".heic",
            ".heif"
        };

        if (!allowedExtensions.Contains(extension))
            throw new Exception("Invalid image type.");

        // =================================================
        // CREATE FOLDER
        // =================================================

        var uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            folderName
        );

        Directory.CreateDirectory(uploadsFolder);

        // =================================================
        // ALWAYS SAVE AS WEBP
        // =================================================

        var finalFileName =
            $"{fileName}-{Guid.NewGuid():N}.webp";

        var filePath = Path.Combine(
            uploadsFolder,
            finalFileName
        );

        await using var inputStream = file.OpenReadStream();

        using var image = new MagickImage(inputStream);

        // Fix EXIF orientation
        image.AutoOrient();

        // Optional: limit huge images
        image.Quality = 82;

        image.Format = MagickFormat.WebP;

        await image.WriteAsync(filePath);

        return $"uploads/{folderName}/{finalFileName}";
    }

    // =====================================================
    // UPLOAD MANY
    // =====================================================

    public async Task<List<string>> UploadManyAsync(
        List<IFormFile> files,
        string folderName)
    {
        var result = new List<string>();

        if (files == null || !files.Any())
            return result;

        int index = 1;

        foreach (var file in files)
        {
            var path = await UploadAsync(
                file,
                folderName,
                $"image-{index:D3}"
            );

            result.Add(path);

            index++;
        }

        return result;
    }

    // =====================================================
    // DELETE ONE IMAGE
    // =====================================================

    public void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            return;

        var cleanPath = imagePath
            .TrimStart('/')
            .Replace("/", Path.DirectorySeparatorChar.ToString());

        var fullPath = Path.Combine(
            _environment.WebRootPath,
            cleanPath
        );

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    // =====================================================
    // DELETE WHOLE PROJECT FOLDER
    // =====================================================

    public void DeleteFolder(string folderName)
    {
        if (string.IsNullOrWhiteSpace(folderName))
            return;

        var folderPath = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            folderName
        );

        if (Directory.Exists(folderPath))
        {
            Directory.Delete(
                folderPath,
                recursive: true
            );
        }
    }

    public Task<string> UploadAsync(string coverImage, string folderName)
    {
        throw new NotImplementedException();
    }
}
