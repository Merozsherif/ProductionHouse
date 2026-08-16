using Microsoft.AspNetCore.Mvc;
using ProductionHouse.API.DTOs;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Responses;

namespace ProductionHouse.API.Controllers
{

    public class UploadController : BaseController
    {
        private readonly IImageService _imageService;

        public UploadController(IImageService imageService)
        {
            _imageService = imageService;
        }

        // ==========================================
        // Upload Project Cover Image
        // URL: /api/Upload/Upload-Project
        // ==========================================
        [HttpPost("Upload-Project")]
        public async Task<IActionResult> UploadProject([FromForm] AddProjectRequest request)
        {
            var imagePath = await _imageService.UploadAsync(
                request.CoverImage,
                "projects");

            return Ok(new ApiResponse<string>(
                true,
                "Image uploaded successfully.",
                imagePath
            ));
        }

        // ==========================================
        // Upload Category Image
        // URL: /api/Upload/Upload-Category
        // ==========================================
        [HttpPost("Upload-Category")]
        public async Task<IActionResult> UploadCategory([FromForm] AddCategoryRequest request)
        {
            var imagePath = await _imageService.UploadAsync(
                request.Image,
                "categories");

            return Ok(new ApiResponse<string>(
                true,
                "Image uploaded successfully.",
                imagePath
            ));
        }

        // ==========================================
        // Test
        // URL: /api/Upload/Test
        // ==========================================
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok(new ApiResponse<string>(
                true,
                "Upload API is working.",
                "OK"
            ));
        }
    }
}