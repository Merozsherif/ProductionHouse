using ProductionHouse.Core.DTOs;


namespace ProductionHouse.Core.Interfaces;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync();
    Task AddAsync(AddProjectDto dto);
    Task<ProjectDto> GetByIdAsync(int id);
    Task UpdateAsync(UpdateProjectDto dto);
    Task DeleteAsync(int id);
}