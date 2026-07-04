using AutoMapper;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Exceptions;
using ProductionHouse.Core.Interfaces;

namespace ProductionHouse.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    // ===================== CREATE =====================
    public async Task AddAsync(AddProjectDto dto)
    {
        var project = _mapper.Map<Project>(dto);

        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();

        if (dto.Translations != null && dto.Translations.Any())
        {
            foreach (var item in dto.Translations)
            {
                var translation = _mapper.Map<ProjectTranslation>(item);
                translation.ProjectId = project.Id;

                await _unitOfWork.ProjectTranslations.AddAsync(translation);
            }

            await _unitOfWork.SaveChangesAsync();
        }
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
    public async Task<List<ProjectDto>> GetAllAsync()
    {
        var projects = await _unitOfWork.Projects.GetAllAsync();

        var result = new List<ProjectDto>();

        foreach (var project in projects)
        {
            var translations = await _unitOfWork.ProjectTranslations
                .FindAsync(x => x.ProjectId == project.Id);

            var dto = _mapper.Map<ProjectDto>(project);

            dto.Translations = _mapper.Map<List<ProjectTranslationDto>>(translations);

            result.Add(dto);
        }

        return result;
    }

    // ===================== GET BY ID =====================
    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);

        if (project == null)
            throw new NotFoundException("Project not found.");

        var translations = await _unitOfWork.ProjectTranslations
            .FindAsync(x => x.ProjectId == project.Id);

        var dto = _mapper.Map<ProjectDto>(project);

        dto.Translations = _mapper.Map<List<ProjectTranslationDto>>(translations);

        return dto;
    }

    // ===================== UPDATE =====================
    public async Task UpdateAsync(UpdateProjectDto dto)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(dto.Id);

        if (project == null)
            throw new NotFoundException("Project not found.");

        _mapper.Map(dto, project);

        _unitOfWork.Projects.Update(project);

        await _unitOfWork.SaveChangesAsync();

        // Translation update can be implemented later.
    }
}