using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

namespace ProductionHouse.Infrastructure.Repositories
{

    public class CategoryRepository
        : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public override async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(x => x.Translations)
                .ToListAsync();
        }

        public override async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
