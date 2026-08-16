using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using ProductionHouse.Core.Interfaces;

namespace ProductionHouse.Infrastructure.Services;

public class ImageService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public ImageService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    // =====================================================
    // UPLOAD IMAGE -> WEBP -> CLOUDINARY
    // =====================================================
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

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var allowedExtensions = new[]
        {
            ".jpg", ".jpeg", ".png", ".webp", ".heic", ".heif"
        };

        if (!allowedExtensions.Contains(extension))
            throw new Exception("Invalid image type.");

        // Convert to WebP in-memory first (نفس منطقك القديم بالظبط)
        await using var inputStream = file.OpenReadStream();
        using var image = new MagickImage(inputStream);

        image.AutoOrient();
        image.Quality = 82;
        image.Format = MagickFormat.WebP;

        using var outputStream = new MemoryStream();
        await image.WriteAsync(outputStream, MagickFormat.WebP);
        outputStream.Position = 0;

        var publicId = $"{folderName}/{fileName}-{Guid.NewGuid():N}";

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription($"{publicId}.webp", outputStream),
            PublicId = publicId,
            Folder = null, // الـ folder متضمن في publicId فوق
            Overwrite = false,
            Format = "webp"
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
            throw new Exception($"Upload failed: {result.Error.Message}");

        // نرجع الرابط الكامل بدل مسار نسبي
        return result.SecureUrl.ToString();
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
            var url = await UploadAsync(file, folderName, $"image-{index:D3}");
            result.Add(url);
            index++;
        }

        return result;
    }

    // =====================================================
    // DELETE ONE IMAGE (بالرابط الكامل)
    // =====================================================
    public void DeleteImage(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            return;

        var publicId = ExtractPublicId(imageUrl);

        if (string.IsNullOrEmpty(publicId))
            return;

        var deleteParams = new DeletionParams(publicId);
        _cloudinary.Destroy(deleteParams);
    }

    // =====================================================
    // DELETE WHOLE PROJECT FOLDER
    // =====================================================
    public void DeleteFolder(string folderName)
    {
        if (string.IsNullOrWhiteSpace(folderName))
            return;

        _cloudinary.DeleteResourcesByPrefix(folderName);
        _cloudinary.DeleteFolder(folderName);
    }

    // =====================================================
    // MOVE / RENAME IMAGE
    // =====================================================
    public void MoveImage(string oldImageUrl, string newRelativePath)
    {
        if (string.IsNullOrWhiteSpace(oldImageUrl))
            return;

        var oldPublicId = ExtractPublicId(oldImageUrl);
        var newPublicId = newRelativePath.Replace(".webp", "");

        if (string.IsNullOrEmpty(oldPublicId))
            return;

        _cloudinary.Rename(oldPublicId, newPublicId, overwrite: true);
    }

    public Task<string> UploadAsync(string coverImage, string folderName)
    {
        throw new NotImplementedException();
    }

    // =====================================================
    // Helper: استخرج الـ public_id من رابط Cloudinary كامل
    // =====================================================
    private string ExtractPublicId(string url)
    {
        // مثال: https://res.cloudinary.com/rjycqoql/image/upload/v123456/projects/image-001-abc.webp
        var uploadIndex = url.IndexOf("/upload/");
        if (uploadIndex == -1) return string.Empty;

        var afterUpload = url.Substring(uploadIndex + "/upload/".Length);

        // شيل الـ version لو موجودة (v123456/)
        var parts = afterUpload.Split('/');
        var startIndex = parts[0].StartsWith("v") && parts[0].Length > 1 && parts[0].Substring(1).All(char.IsDigit)
            ? 1
            : 0;

        var publicIdWithExtension = string.Join("/", parts.Skip(startIndex));
        return Path.ChangeExtension(publicIdWithExtension, null);
    }
}