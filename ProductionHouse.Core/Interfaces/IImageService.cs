using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductionHouse.Core.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadAsync(
            IFormFile file,
            string folderName,
            string fileName = "image");

        Task<List<string>> UploadManyAsync(
            List<IFormFile> files,
            string folderName);

        void DeleteImage(string? imagePath);
        // في IImageService.cs
        void MoveImage(string oldRelativePath, string newRelativePath);
        void DeleteFolder(string folderName);
        Task<string> UploadAsync(string coverImage, string folderName);
    }
}
