using ProductionHouse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        IProjectRepository Projects { get; }

        IGenericRepository<ProjectImage> ProjectImages { get; }

        IGenericRepository<ProjectTranslation> ProjectTranslations { get; }

        IGenericRepository<Category> Categories { get; }

        IGenericRepository<CategoryTranslation> CategoryTranslations { get; }
        IAdminRepository Admins { get; }
        Task<int> SaveChangesAsync();
    }
}
