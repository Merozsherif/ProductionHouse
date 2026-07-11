using AutoMapper;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Exceptions;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Interfaces.ProductionHouse.Core.Interfaces;

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


    // ===================== CREATE =====================
    public async Task AddAsync(AddProjectDto request)
    {
        // Create Project
        var project = new Project
        {
            CategoryId = request.CategoryId,
            ClientName = request.ClientName,
            CoverImage = request.CoverImage
        };

        await _unitOfWork.Projects.AddAsync(project);

        await _unitOfWork.SaveChangesAsync();

        // Save Translations
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

        // Save Gallery Images
        if (request.GalleryImages != null && request.GalleryImages.Any())
        {
            await AddGalleryImagesAsync(
                project.Id,
                request.GalleryImages);
        }

        await _unitOfWork.SaveChangesAsync();
    }

    // ===================== DELETE =====================
    public async Task DeleteAsync(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);

        if (project == null)
            throw new NotFoundException("Project not found.");

        _unitOfWork.Projects.Delete(project);

        await _unitOfWork.SaveChangesAsync();
    }

    // ===================== GET ALL =====================
    //public async Task<List<ProjectDto>> GetAllAsync()
    //{
    //    var projects =
    //        await _unitOfWork.Projects.GetAllAsync();

    //    return _mapper.Map<List<ProjectDto>>(projects);
    //}
    public async Task<List<ProjectDto>> GetAllAsync()
    {
        var projects = await _unitOfWork.Projects.GetAllAsync();

        foreach (var project in projects)
        {
            Console.WriteLine($"Project Id = {project.Id}");
            Console.WriteLine($"Images Count = {project.Images.Count}");

            foreach (var image in project.Images)
            {
                Console.WriteLine(image.ImageUrl);
            }
        }

        return _mapper.Map<List<ProjectDto>>(projects);
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
        var project =
            await _unitOfWork.Projects.GetByIdAsync(dto.Id);

        if (project == null)
            throw new NotFoundException("Project not found");

        project.ClientName = dto.ClientName;
        project.CategoryId = dto.CategoryId;

        if (!string.IsNullOrEmpty(dto.CoverImage))
        {
            project.CoverImage = dto.CoverImage;
        }

        _unitOfWork.Projects.Update(project);

        await _unitOfWork.SaveChangesAsync();

        // Translation
        var translations =
            await _unitOfWork.ProjectTranslations
            .FindAsync(x => x.ProjectId == dto.Id);

        foreach (var old in translations)
        {
            _unitOfWork.ProjectTranslations.Delete(old);
        }

        foreach (var item in dto.Translations)
        {
            await _unitOfWork.ProjectTranslations
            .AddAsync(new ProjectTranslation
            {
                ProjectId = project.Id,
                LanguageCode = item.LanguageCode,
                Title = item.Title,
                Description = item.Description
            });
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
            throw new NotFoundException("Image not found.");

        _unitOfWork.ProjectImages.Delete(image);

        await _unitOfWork.SaveChangesAsync();
    }
}