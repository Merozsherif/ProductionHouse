using Microsoft.EntityFrameworkCore;
using ProductionHouse.Core.Entities;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Infrastructure.Repositories
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}
