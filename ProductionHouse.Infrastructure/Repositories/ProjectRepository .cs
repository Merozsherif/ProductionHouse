using Microsoft.EntityFrameworkCore;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Infrastructure.Data;

namespace ProductionHouse.Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetProjectsWithCategoryAsync()
        {
            return await _context.Projects
                .Include(p => p.Category)
                .ToListAsync();
        }
    }
}