using ProductionHouse.Core.DTOs;

namespace ProductionHouse.Core.Interfaces;

public interface IProjectService
{
    Task<PagedResult<ProjectDto>>GetAllAsync(ProjectQueryDto query);
    Task<int> MigrateOldImagesAsync();
    Task<ProjectDto?> GetByIdAsync(int id);
    Task DeleteGalleryImageAsync(int imageId);
    Task AddAsync(AddProjectDto dto);
    Task ChangeCoverAsync(int projectId, string coverImage);
    Task UpdateAsync(UpdateProjectDto dto);

    Task DeleteAsync(int id);

    Task AddGalleryImagesAsync(
        int projectId,
        List<string> images);
    void MoveImage(string oldRelativePath, string newRelativePath); // ✅ ضيف السطر ده
}