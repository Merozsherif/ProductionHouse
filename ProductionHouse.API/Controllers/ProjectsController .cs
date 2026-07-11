using Microsoft.AspNetCore.Mvc;
using ProductionHouse.API.DTOs;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Enums;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Interfaces.ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Responses;

namespace ProductionHouse.API.Controllers
{
    public class ProjectsController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IImageService _imageService;

        public ProjectsController(
            IProjectService projectService,
            IImageService imageService)
        {
            _projectService = projectService;
            _imageService = imageService;
        }

        // ===================== GET ALL =====================
        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllAsync();

            return Ok(new ApiResponse<List<ProjectDto>>(
                true,
                "Projects retrieved successfully.",
                projects
            ));
        }

        // ===================== GET BY ID =====================
        [HttpGet("Get-By-Id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);

            return Ok(new ApiResponse<ProjectDto>(
                true,
                "Project retrieved successfully.",
                project
            ));
        }

        // ===================== ADD =====================
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddProjectRequest request)
        {
            // Upload Cover
            var coverImage = await _imageService.UploadAsync(
                request.CoverImage,
                "projects");

            // Upload Gallery
            var galleryImages = await _imageService.UploadManyAsync(
                request.GalleryImages,
                "projects");

            // Convert Request -> DTO
            var dto = MapToDto(
                request,
                coverImage,
                galleryImages);

            // Save
            await _projectService.AddAsync(dto);

            return Ok(new ApiResponse<string>(
                true,
                "Project created successfully."
            ));
        }

        // ===================== UPDATE =====================
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(
      int id,
      [FromForm] UpdateProjectRequest request)
        {
            string? cover = null;

            if (request.CoverImage != null)
            {
                cover = await _imageService.UploadAsync(
                    request.CoverImage,
                    "projects");
            }

            List<string> gallery = new();

            if (request.GalleryImages != null &&
                request.GalleryImages.Any())
            {
                gallery = await _imageService.UploadManyAsync(
                    request.GalleryImages,
                    "projects");
            }

            var dto = new UpdateProjectDto
            {
                Id = id,
                CategoryId = request.CategoryId,
                ClientName = request.ClientName,
                CoverImage = cover,
                GalleryImages = gallery,

                Translations = new()
        {
            new ProjectTranslationDto
            {
                LanguageCode=LanguageCode.EN,
                Title=request.Title,
                Description=request.Description
            }
        }
            };

            await _projectService.UpdateAsync(dto);

            return Ok(new ApiResponse<string>(
                true,
                "Project updated successfully."));
        }

        // ===================== DELETE =====================
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.DeleteAsync(id);

            return Ok(new ApiResponse<string>(
                true,
                "Project deleted successfully."
            ));
        }

        // ===================== Upload Gallery =====================
        [HttpPost("Upload-Gallery/{id:int}")]
        public async Task<IActionResult> UploadGallery(
            int id,
            [FromForm] UploadGalleryRequest request)
        {
            var images = await _imageService.UploadManyAsync(
                request.Images,
                "projects");

            await _projectService.AddGalleryImagesAsync(
                id,
                images);

            return Ok(new ApiResponse<List<string>>(
                true,
                "Gallery uploaded successfully.",
                images
            ));
        }



        // ===================== CHANGE COVER =====================
        // URL: api/Projects/Change-Cover/5
        [HttpPut("Change-Cover/{id:int}")]
        public async Task<IActionResult> ChangeCover(
            int id,
            [FromForm] ChangeCoverRequest request)
        {
            var cover = await _imageService.UploadAsync(
                request.CoverImage,
                "projects");

            await _projectService.ChangeCoverAsync(
                id,
                cover);

            return Ok(new ApiResponse<string>(
                true,
                "Cover updated successfully."
            ));
        }
        // ===================== DELETE GALLERY IMAGE =====================
        // URL: api/Projects/Delete-Gallery-Image/15


        [HttpDelete("Delete-Gallery-Image/{imageId:int}")]
        public async Task<IActionResult> DeleteGalleryImage(int imageId)
        {
            await _projectService.DeleteGalleryImageAsync(imageId);

            return Ok(new ApiResponse<string>(
                true,
                "Image deleted successfully."
            ));
        }
        // =====================================================
        // Private Methods
        // =====================================================

        private AddProjectDto MapToDto(
            AddProjectRequest request,
            string coverImage,
            List<string> galleryImages)
        {
            return new AddProjectDto
            {
                CategoryId = request.CategoryId,

                ClientName = request.ClientName,

                CoverImage = coverImage,

                GalleryImages = galleryImages,

                Translations = new List<ProjectTranslationDto>
                {
                    new ProjectTranslationDto
                    {
                     LanguageCode = LanguageCode.EN,
                        Title = request.Title,
                        Description = request.Description
                    }
                }
            };
        }
    }
}