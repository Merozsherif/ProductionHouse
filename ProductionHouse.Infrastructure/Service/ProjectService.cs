using AutoMapper;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Enums;
using ProductionHouse.Core.Exceptions;
using ProductionHouse.Core.Interfaces;

namespace ProductionHouse.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IImageService _imageService;

    public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _imageService = imageService;
    }

    private static string CreateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "project";

        var slug = text
            .ToLowerInvariant()
            .Trim();

        slug = System.Text.RegularExpressions.Regex
            .Replace(slug, @"[^a-z0-9]+", "-");

        slug = slug.Trim('-');

        return string.IsNullOrWhiteSpace(slug)
            ? "project"
            : slug;
    }

    // ===================== CREATE =====================
    public async Task AddAsync(AddProjectDto request)
    {
        // 1) نعمل الـ project الأول من غير صور عشان ناخد الـ Id
        var project = new Project
        {
            CategoryId = request.CategoryId,
            ClientName = request.ClientName
        };

        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.SaveChangesAsync(); // ✅ دلوقتي project.Id موجود

        var folderName = $"projects/{project.Id}";

        // 2) رفع الـ Cover
        if (request.CoverImage != null && request.CoverImage.Length > 0)
        {
            project.CoverImage = await _imageService.UploadAsync(
                request.CoverImage,
                folderName);

            _unitOfWork.Projects.Update(project);
        }

        // 3) رفع الـ Gallery
        if (request.GalleryImages != null && request.GalleryImages.Any())
        {
            var gallery = await _imageService.UploadManyAsync(
                request.GalleryImages,
                folderName);

            await AddGalleryImagesAsync(project.Id, gallery);
        }

        // 4) الترجمات
        if (request.Translations != null && request.Translations.Any())
        {
            foreach (var item in request.Translations)
            {
                await _unitOfWork.ProjectTranslations.AddAsync(
                    new ProjectTranslation
                    {
                        ProjectId = project.Id,
                        LanguageCode = item.LanguageCode,
                        Title = item.Title,
                        Description = item.Description
                    });
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }

    //saff

    public async Task<int> MigrateOldImagesAsync()
    {
        var projects = await _unitOfWork.Projects.GetAllWithImagesAsync(); // ✅ بدل GetProjectsWithCategoryAsync

        int movedCount = 0;

        foreach (var project in projects)
        {
            // Cover Image
            if (!string.IsNullOrWhiteSpace(project.CoverImage))
            {
                var fileName = Path.GetFileName(project.CoverImage);
                var newPath = $"uploads/projects/{project.Id}/{fileName}";

                if (project.CoverImage != newPath)
                {
                    _imageService.MoveImage(project.CoverImage, newPath);
                    project.CoverImage = newPath;
                    _unitOfWork.Projects.Update(project);
                    movedCount++;
                }
            }

            // Gallery Images
            if (project.Images != null && project.Images.Any())
            {
                foreach (var image in project.Images)
                {
                    var fileName = Path.GetFileName(image.ImageUrl);
                    var newPath = $"uploads/projects/{project.Id}/{fileName}";

                    if (image.ImageUrl != newPath)
                    {
                        _imageService.MoveImage(image.ImageUrl, newPath);
                        image.ImageUrl = newPath;
                        _unitOfWork.ProjectImages.Update(image);
                        movedCount++;
                    }
                }
            }
        }

        await _unitOfWork.SaveChangesAsync();

        return movedCount;
    }
    // ===================== DELETE =====================
    public async Task DeleteAsync(int id)
    {
        var project = await _unitOfWork
            .Projects
            .GetByIdAsync(id);

        if (project == null)
            throw new NotFoundException(
                "Project not found."
            );

        // =========================
        // Delete physical folder
        // =========================

        _imageService.DeleteFolder(
            $"projects/{project.Id}"
        );

        // =========================
        // Delete from database
        // =========================

        _unitOfWork.Projects.Delete(project);

        await _unitOfWork.SaveChangesAsync();
    }

    // ===================== GET ALL =====================
    public async Task<PagedResult<ProjectDto>>
    GetAllAsync(ProjectQueryDto query)
    {
        var result =
            await _unitOfWork
            .Projects
            .GetPagedAsync(query);

        return new PagedResult<ProjectDto>
        {
            Items =
                _mapper.Map<List<ProjectDto>>(result.Items),

            TotalCount = result.TotalCount,

            Page = result.Page,

            PageSize = result.PageSize
        };
    }

    // ===================== GET BY ID =====================
    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project =
            await _unitOfWork.Projects.GetByIdAsync(id);

        if (project == null)
            throw new NotFoundException("Project not found");

        return _mapper.Map<ProjectDto>(project);
    }

    // ===================== UPDATE =====================
    public async Task UpdateAsync(UpdateProjectDto dto)
    {
        // ===========================
        // Validation
        // ===========================

        if (string.IsNullOrWhiteSpace(dto.ClientName))
            throw new BadRequestException("Client Name is required.");

        if (dto.CategoryId <= 0)
            throw new BadRequestException("Invalid Category.");

        if (dto.Translations == null || !dto.Translations.Any())
            throw new BadRequestException(
                "Project must contain at least one translation.");

        // التحقق من وجود القسم
        var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);

        if (category == null)
            throw new NotFoundException("Category not found.");

        // التحقق من وجود المشروع
        var project = await _unitOfWork.Projects.GetByIdAsync(dto.Id);

        if (project == null)
            throw new NotFoundException("Project not found.");

        // ==================================
        // يبدأ التعديل
        // ==================================

        project.ClientName = dto.ClientName;
        project.CategoryId = dto.CategoryId;

        if (!string.IsNullOrWhiteSpace(dto.CoverImage))
        {
            if (!string.IsNullOrWhiteSpace(project.CoverImage))
            {
                _imageService.DeleteImage(project.CoverImage);
            }

            project.CoverImage = dto.CoverImage;
        }

        _unitOfWork.Projects.Update(project);

        await _unitOfWork.SaveChangesAsync();

        // Add Gallery Images
        if (dto.GalleryImages.Any())
        {
            await AddGalleryImagesAsync(
                project.Id,
                dto.GalleryImages);
        }

        // Delete Gallery Images
        if (dto.DeletedGalleryImages.Any())
        {
            foreach (var imageId in dto.DeletedGalleryImages)
            {
                await DeleteGalleryImageAsync(imageId);
            }
        }

        // Update Translations
        var translations =
            await _unitOfWork.ProjectTranslations
            .FindAsync(x => x.ProjectId == dto.Id);

        foreach (var item in dto.Translations)
        {
            var translation = translations
                .FirstOrDefault(x => x.LanguageCode == item.LanguageCode);

            if (translation == null)
            {
                await _unitOfWork.ProjectTranslations.AddAsync(
                    new ProjectTranslation
                    {
                        ProjectId = project.Id,
                        LanguageCode = item.LanguageCode,
                        Title = item.Title,
                        Description = item.Description
                    });
            }
            else
            {
                translation.Title = item.Title;
                translation.Description = item.Description;
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }

    // ===================== ADD GALLERY =====================
    public async Task AddGalleryImagesAsync(
        int projectId,
        List<string> images)
    {
        foreach (var image in images)
        {
            await _unitOfWork.ProjectImages.AddAsync(
                new ProjectImage
                {
                    ProjectId = projectId,
                    ImageUrl = image
                });
        }

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ChangeCoverAsync(
        int projectId,
        string coverImage)
    {
        var project = await _unitOfWork
            .Projects
            .GetByIdAsync(projectId);

        if (project == null)
            throw new NotFoundException("Project not found.");

        project.CoverImage = coverImage;

        _unitOfWork.Projects.Update(project);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteGalleryImageAsync(int imageId)
    {
        var image =
            await _unitOfWork.ProjectImages
            .GetByIdAsync(imageId);

        if (image == null)
            throw new NotFoundException(
                "Image not found."
            );

        // Delete physical image
        _imageService.DeleteImage(
            image.ImageUrl
        );

        // Delete DB record
        _unitOfWork.ProjectImages.Delete(image);

        await _unitOfWork.SaveChangesAsync();
    }

    public void MoveImage(string oldRelativePath, string newRelativePath)
    {
        throw new NotImplementedException();
    }
}
