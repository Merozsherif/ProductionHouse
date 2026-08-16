using ProductionHouse.Core.DTOs;
using ProductionHouse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionHouse.Core.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<List<Project>> GetAllWithImagesAsync();
        Task<List<Project>> GetProjectsWithCategoryAsync();
        Task<PagedResult<Project>> GetPagedAsync(ProjectQueryDto query);
        Task AddGalleryImagesAsync(List<ProjectImage> images);
    }
}
 