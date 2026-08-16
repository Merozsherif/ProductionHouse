using Microsoft.EntityFrameworkCore;
using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Enums;
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
    public async Task<List<Project>> GetAllWithImagesAsync()
    {
        return await _context.Projects
            .Include(x => x.Images)
            .ToListAsync();
    }
    public async Task AddGalleryImagesAsync(List<ProjectImage> images)
    {
        await _context.ProjectImages.AddRangeAsync(images);

        await _context.SaveChangesAsync();
    }

    public  async Task<PagedResult<Project>> GetPagedAsync(ProjectQueryDto query)
    {
        IQueryable<Project> projects = _context.Projects
            .Include(x => x.Images)
            .Include(x => x.Category)
                .ThenInclude(x => x.Translations)
                .Include(x => x.Translations);

            //search 
            if(!string.IsNullOrEmpty(query.Search))
            {
                projects = projects.Where(x 
                    => x.Translations.Any(t => t.Title.Contains(query.Search)));
            }
            //Category filter
            if(query.CategoryId.HasValue)
            {
                projects = projects.Where(x => x.CategoryId == query.CategoryId.Value);
            }

            if(query.IsPublished.HasValue)
            {
                projects = projects.Where(x => x.IsPublished == query.IsPublished.Value);
            }

            // ================= Featured =================
            if (query.IsFeatured.HasValue)
            {
                projects = projects.Where(x =>
                    x.IsFeatured == query.IsFeatured);
            }

        // ================= Sorting =================
        switch (query.SortBy)
        {
            case ProjectSortBy.ClientName:
                projects = query.Desc
                    ? projects.OrderByDescending(x => x.ClientName)
                    : projects.OrderBy(x => x.ClientName);
                break;

            case ProjectSortBy.DisplayOrder:
                projects = query.Desc
                    ? projects.OrderByDescending(x => x.DisplayOrder)
                    : projects.OrderBy(x => x.DisplayOrder);
                break;

            default:
                projects = query.Desc
                    ? projects.OrderByDescending(x => x.CreatedAt)
                    : projects.OrderBy(x => x.CreatedAt);
                break;
        }
        // ================= Count =================
        var total = await projects.CountAsync();

        // ================= Pagination =================
        var items = await projects
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return new PagedResult<Project>
        {
            Items = items,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        };
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