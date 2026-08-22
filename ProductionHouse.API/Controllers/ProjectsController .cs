using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionHouse.API.DTOs;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Enums;
using ProductionHouse.Core.Interfaces;
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
        [AllowAnonymous]
        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAll(
            [FromQuery] ProjectQueryDto query)
        {
            var projects =
                await _projectService
                .GetAllAsync(query);

            return Ok(new ApiResponse<PagedResult<ProjectDto>>
            (
                true,
                "Projects retrieved successfully.",
                projects
            ));
        }

        // ===================== GET BY ID =====================
        [AllowAnonymous]
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
        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddProjectRequest request)
        {
            var dto = MapToDto(request);

            await _projectService.AddAsync(dto);

            return Ok(new ApiResponse<string>(true, "Project created successfully."));
        }

        private AddProjectDto MapToDto(AddProjectRequest request)
        {
            return new AddProjectDto
            {
                CategoryId = request.CategoryId,
                ClientName = request.ClientName,
                CoverImage = request.CoverImage,
                GalleryImages = request.GalleryImages,
                Translations = new()
        {
            new ProjectTranslationDto
            {
                LanguageCode = LanguageCode.EN,
                Title = request.TitleEn,
                Description = request.DescriptionEn
            },
            new ProjectTranslationDto
            {
                LanguageCode = LanguageCode.AR,
                Title = request.TitleAr,
                Description = request.DescriptionAr
            }
        }
            };
        }
        // ===================== UPDATE =====================
        [Authorize(Roles = "Admin")]
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
                DeletedGalleryImages = request.DeletedGalleryImages,
                Translations = new()
    {
        new ProjectTranslationDto
        {
            LanguageCode = LanguageCode.EN,
            Title = request.TitleEn,
            Description = request.DescriptionEn
        },
        new ProjectTranslationDto
        {
            LanguageCode = LanguageCode.AR,
            Title = request.TitleAr,
            Description = request.DescriptionAr
        }
    }
            };

            await _projectService.UpdateAsync(dto);

            return Ok(new ApiResponse<string>(
                true,
                "Project updated successfully."));
        }

        // ===================== DELETE =====================
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete-Gallery-Image/{imageId:int}")]
        public async Task<IActionResult> DeleteGalleryImage(int imageId)
        {
            await _projectService.DeleteGalleryImageAsync(imageId);

            return Ok(new ApiResponse<string>(
                true,
                "Image deleted successfully."
            ));
        }
        // ===================== ONE-TIME MIGRATION =====================
        // بعد التشغيل مرة واحدة، امسح الـ endpoint ده
        [Authorize(Roles = "Admin")]
        [HttpPost("Migrate-Old-Images")]
        public async Task<IActionResult> MigrateOldImages()
        {
            var result = await _projectService.MigrateOldImagesAsync();

            return Ok(new ApiResponse<string>(
                true,
                $"Migration done. Moved {result} files."
            ));
        }
        // =====================================================
        // Private Methods
        // =====================================================
        //private AddProjectDto MapToDto(AddProjectRequest request)
        //{
        //    return new AddProjectDto
        //    {
        //        CategoryId = request.CategoryId,
        //        ClientName = request.ClientName,
        //        CoverImage = request.CoverImage,
        //        GalleryImages = request.GalleryImages,
        //        Translations = new()
        //{
        //    new ProjectTranslationDto
        //    {
        //        LanguageCode = LanguageCode.EN,
        //        Title = request.Title,
        //        Description = request.Description
        //    }
        //}
        //    };
        //}
    }
}
