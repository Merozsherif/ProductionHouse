using ProductionHouse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.Interfaces
{
    public interface IAdminRepository : IGenericRepository<Admin>
    {
        Task<Admin?> GetByEmailAsync(string email);
    }
}

