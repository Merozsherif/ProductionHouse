using Microsoft.EntityFrameworkCore;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Infrastructure.Data;

namespace ProductionHouse.Infrastructure.Repositories;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetProjectsWithCategoryAsync()
    {
        return await _context.Projects
            .Include(x => x.Category)
            .ToListAsync();
    }

    public async Task AddGalleryImagesAsync(List<ProjectImage> images)
    {
        await _context.ProjectImages.AddRangeAsync(images);

        await _context.SaveChangesAsync();
    }

    public override async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
         .Include(x => x.Images)
         .Include(x => x.Category)
             .ThenInclude(x => x.Translations)
         .Include(x => x.Translations)
         .ToListAsync();
    }

    public override async Task<Project?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .Include(x => x.Images)
            .Include(x => x.Category)
                .ThenInclude(x => x.Translations)
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}