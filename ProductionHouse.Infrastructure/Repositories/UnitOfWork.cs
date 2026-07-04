using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Infrastructure.Data;
using ProductionHouse.Infrastructure.Repositories;

namespace ProductionHouse.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IProjectRepository Projects { get; }

    public IGenericRepository<ProjectImage> ProjectImages { get; }

    public IGenericRepository<ProjectTranslation> ProjectTranslations { get; }

    public IGenericRepository<Category> Categories { get; }

    public IGenericRepository<CategoryTranslation> CategoryTranslations { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        Projects = new ProjectRepository(context);

        ProjectImages =
            new GenericRepository<ProjectImage>(context);

        ProjectTranslations =
            new GenericRepository<ProjectTranslation>(context);

        Categories =
            new GenericRepository<Category>(context);

        CategoryTranslations =
            new GenericRepository<CategoryTranslation>(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}