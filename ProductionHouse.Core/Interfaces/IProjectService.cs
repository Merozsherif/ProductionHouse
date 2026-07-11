using ProductionHouse.Core.DTOs;

namespace ProductionHouse.Core.Interfaces;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync();

    Task<ProjectDto?> GetByIdAsync(int id);
    Task DeleteGalleryImageAsync(int imageId);
    Task AddAsync(AddProjectDto dto);
    Task ChangeCoverAsync(int projectId, string coverImage);
    Task UpdateAsync(UpdateProjectDto dto);

    Task DeleteAsync(int id);

    Task AddGalleryImagesAsync(
        int projectId,
        List<string> images);
}